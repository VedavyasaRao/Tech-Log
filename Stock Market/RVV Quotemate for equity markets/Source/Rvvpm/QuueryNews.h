#include <sddl.h>
#include "SimpleIPC.h"
#include <iostream>
#include <string>
#include <memory>
#pragma once
string extract_retvalue(const string& jsonstr);


struct GetQuotes
{
    unique_ptr<SimpleIPC::GenericProxy> quotesp;

    GetQuotes(wstring servername)
    {
        quotesp.reset(new SimpleIPC::GenericProxy(new SimpleIPC::Windows::SIPCProxy(servername)));
        ::Sleep(5000);
    }

    string QueryDateTime()
    {
        auto ret = extract_retvalue(quotesp->InvokeMember("QueryDateTime"));
        return ret;
    }

    string QueryIndicies()
    {
        auto ret = extract_retvalue(quotesp->InvokeMember("QueryIndicies"));
        return ret;
    }

    string QueryCloses(string ticker)
    {
        auto ret = extract_retvalue(quotesp->InvokeMember("QueryCloses", ticker));
        return ret;

    }

    string QueryQuote(string ticker)
    {
        auto ret = extract_retvalue(quotesp->InvokeMember("QueryQuote", ticker));
        return ret;

    }
};

struct GetNews
{
    unique_ptr<SimpleIPC::GenericProxy> newssp;

    GetNews()
    {
        newssp.reset(new SimpleIPC::GenericProxy(new SimpleIPC::Windows::SIPCProxy(L"GoogleNewsServer")));
        ::Sleep(5000);
    }

    string QueryNews(string ticker)
    {
        return extract_retvalue(newssp->InvokeMember("QueryNews", ticker));
    }

};
