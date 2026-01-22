#pragma once
#pragma region SimpleIPC
namespace  SimpleIPC
{
    // Utility to convert value to JSON-safe string
    template<typename T>
    string to_json_value(const T& value);

    // Base case: no arguments
    void serialize_impl(ostringstream&);

    // Recursive case: serialize key-value pairs
    template<typename T, typename... Args>
    void serialize_impl(ostringstream& out, const string& key, const T& value, const Args&... rest);

    // Entry point
    template<typename... Args>
    string serialize_json(const Args&... args);

#pragma region MemoryManager
    struct MemoryManager
    {
        enum class SIPCArgsIndex :int { Name, Exception, Returnval, Args };
        enum SIPCEncoding : ::byte { binary, json };

        const unsigned BUFFER_OFFSET = 1;
        const unsigned BUFFER_SIZE = 4094 * 4;
        HANDLE mtx = nullptr;
        HANDLE evt = nullptr;

        wstring mmfname;
        HANDLE dataptr = nullptr;
        HANDLE hmmf = nullptr;

        wstring _name;
        SECURITY_ATTRIBUTES  sa;

        SIPCEncoding encoding;

        ::byte* data()
        {
            return (::byte*)dataptr;
        }

        ::byte* data_with_offset()
        {
            return (data() + BUFFER_OFFSET);
        }

        MemoryManager(wstring name)
        {
            _name = name;
            ULONG sz;
            sa.nLength = sizeof(SECURITY_ATTRIBUTES);
            sa.lpSecurityDescriptor = nullptr;
            sa.bInheritHandle = 0;
            ConvertStringSecurityDescriptorToSecurityDescriptorA("D:(A;OICI;GA;;;WD)", 1, &sa.lpSecurityDescriptor, &sz);
            //ms.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow));
            //ws.AddAccessRule(new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.FullControl, AccessControlType.Allow));
        }

        void OpenExisitng()
        {
            bool b;
            wstring temps = L"Global\\mtx" + _name;
            mtx = OpenMutexW(MUTEX_ALL_ACCESS, TRUE, temps.c_str());

            temps = L"Global\\evt" + _name;
            evt = OpenEventW(EVENT_ALL_ACCESS, TRUE, temps.c_str());

            mmfname = L"Global\\mmf" + _name;
            hmmf = CreateFileMappingW(HANDLE(-1), &sa, PAGE_READWRITE, 0, BUFFER_SIZE, mmfname.c_str());
            dataptr = MapViewOfFile(hmmf, 0x1f, 0, 0, 0);
            b = VirtualLock(dataptr, BUFFER_SIZE);

            encoding = (SIPCEncoding)(*data());
        }

        void StoreParameters(string jsoncallargs)
        {
            int off = sizeof(unsigned);
            unsigned sz = jsoncallargs.length();
            memcpy(data_with_offset(), &sz, off);

            memcpy(data_with_offset() + sizeof(unsigned), jsoncallargs.data(), jsoncallargs.length());
        }

        string RetriveParameters()
        {
            unsigned* psz = (unsigned*)data_with_offset();
            auto jsoncallargs = string(((const char*)data_with_offset() + sizeof(unsigned)), *psz);
            return jsoncallargs;
        }

        void BeginCall()
        {
            try
            {
                ResetEvent(evt);
                WaitForSingleObject(mtx, INFINITE);
            }
            catch (...)
            {
            }
        }

        void WaitProcessdone()
        {
            WaitForSingleObject(evt, INFINITE);
        }

        void SetProcessdone()
        {
            SetEvent(evt);
        }

        void EndCall()
        {
            ReleaseMutex(mtx);
        }

        void Close()
        {
            CloseHandle(mtx);
            CloseHandle(evt);
            UnmapViewOfFile(dataptr);
            CloseHandle(hmmf);
        }
    };
#pragma endregion MemoryManager



#pragma region Proxy
    struct ProxyBase
    {
        wstring UniqueName;
        ProxyBase(wstring uniquename) :UniqueName(uniquename) {}
        virtual void PostProcessMessage() {};
        virtual void Close() {};
        virtual bool IsServerAlive() { return false; };
    };

    enum Callmode { sync, async };

    struct GenericProxy
    {
        unique_ptr <ProxyBase> provider;
        unique_ptr<MemoryManager> mm;
        Callmode callmode = Callmode::sync;
        Json::Value root;

        GenericProxy(ProxyBase* provider)
        {
            this->provider.reset(provider);
            mm.reset(new MemoryManager(provider->UniqueName));
            mm->OpenExisitng();
        }

        // Utility to convert value to JSON-safe string
        template<typename T>
        string to_json_value(const T& value)
        {
            if constexpr (std::is_same_v<T, string>)
                return value;
            else if constexpr (std::is_same_v<T, const char*>)
                return  string(value);
            else if constexpr (std::is_same_v<T, bool>)
                return value ? "true" : "false";
            else if constexpr (std::is_arithmetic_v<T>)
                return to_wstring(value); // for int, float, double, etc.

            return "unsupported";
        }

        void AppendParameters()
        {
        }

        template <typename T, typename... Args>
        void AppendParameters(T arg, Args... args)
        {
            Json::Value val(to_json_value(arg));
            root.append(val);
            AppendParameters(args...);
        }

        template <typename... Args>
        string InvokeMember(string mtdname, Args... args)
        {
            root = Json::Value(Json::arrayValue);
            root.append(mtdname);
            root.append(Json::Value::nullSingleton());
            root.append(Json::Value::nullSingleton());

            AppendParameters(args...);

            Json::StreamWriterBuilder builder;
            string callargs = Json::writeString(builder, root);

            mm->BeginCall();
            mm->StoreParameters(callargs);
            provider->PostProcessMessage();
            mm->WaitProcessdone();
            string result = mm->RetriveParameters();
            mm->EndCall();
            return result;
        }
    };
#pragma endregion Proxy


#pragma region NamedObject
    namespace NamedObject
    {
        struct SIPCProxy : public ProxyBase
        {
            HANDLE evtserver;
            SIPCProxy(wstring uniquename) : ProxyBase(uniquename)
            {
                try
                {
                    wstring temps = uniquename;
                    evtserver = OpenEventW(EVENT_ALL_ACCESS, TRUE, temps.c_str());
                }
                catch (...)
                {
                }
            }

            virtual void PostProcessMessage()
            {
                SetEvent(evtserver);
            }

            virtual void Close()
            {
                CloseHandle(evtserver);
            }

            virtual bool IsServerAlive()
            {
                return (evtserver != nullptr);
            }

        };
    }
#pragma endregion NamedObject


#pragma region Windows
    namespace Windows
    {
        struct ShadowWindow
        {
            const unsigned WM_PROCESS_CMD = WM_USER + 3;

            HWND windowHandle;
            WNDCLASS wind_class = {};
            unsigned short classatom;
            wstring classname;
            wstring winname;

            void GetServerWindowHandle(wstring uniquename)
            {
                windowHandle = FindWindowW(nullptr, uniquename.c_str());
            }

            void PostProcessMessage()
            {
                SendNotifyMessage(windowHandle, WM_PROCESS_CMD, NULL, NULL);
            }

            void Close()
            {
                windowHandle = nullptr;
            }

            bool IsServerAlive()
            {
                return (windowHandle != nullptr);
            }
        };

        struct SIPCProxy : public ProxyBase
        {
            ShadowWindow cw;
            SIPCProxy(wstring uniquename) : ProxyBase(uniquename)
            {
                cw.GetServerWindowHandle(uniquename);
            }

            virtual void PostProcessMessage()
            {
                cw.PostProcessMessage();
            }

            virtual void Close()
            {
                cw.Close();
            }

            virtual  bool IsServerAlive()
            {
                return cw.IsServerAlive();
            }
        };
    }
#pragma endregion Windows

};
#pragma endregion SimpleIPC
