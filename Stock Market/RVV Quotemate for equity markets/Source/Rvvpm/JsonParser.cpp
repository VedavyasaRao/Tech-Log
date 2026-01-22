#include "StdAfx.h"
#include <iostream>
#include <sstream>
#include <string>
#include <type_traits>

string extract_retvalue(const string& jsonstr)
{
    JSONCPP_STRING err;
    Json::Value root;
    auto jsonstrlen = jsonstr.length();
    Json::CharReaderBuilder builder;
    const unique_ptr<Json::CharReader> reader(builder.newCharReader());
    if (!reader->parse(jsonstr.c_str(), jsonstr.c_str() + jsonstrlen, &root, &err))
        return "";
    if (!root[1].asString().empty())
        return "";
    return root[2].asString();

}

// Utility to convert value to JSON-safe string
template<typename T>
std::string to_json_value(const T& value) {
    if constexpr (std::is_same_v<T, std::string>)
        return "\"" + value + "\"";
    else if constexpr (std::is_same_v<T, const char*>)
        return "\"" + std::string(value) + "\"";
    else if constexpr (std::is_same_v<T, bool>)
        return value ? "true" : "false";
    else
        return std::to_string(value); // for int, float, double, etc.
}

// Base case: no arguments
void serialize_impl(std::ostringstream&) {}

// Recursive case: serialize key-value pairs
template<typename T, typename... Args>
void serialize_impl(std::ostringstream& out, const std::string& key, const T& value, const Args&... rest) {
    out << "\"" << key << "\":" << to_json_value(value);
    if constexpr (sizeof...(rest) > 0) {
        out << ",";
        serialize_impl(out, rest...);
    }
}

// Entry point
template<typename... Args>
std::string serialize_json(const Args&... args) {
    static_assert(sizeof...(args) % 2 == 0, "Arguments must be in key-value pairs.");
    std::ostringstream out;
    out << "{";
    serialize_impl(out, args...);
    out << "}";
    return out.str();
}
