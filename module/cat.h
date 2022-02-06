#pragma once
#include <initializer_list>

#include <type_traits>
#include <iostream>
#include <assert.h>

// we cannot return a char array from a function, therefore we need a wrapper
template <unsigned N>
struct String {
    char c[N];
};

template<unsigned ...Len>
constexpr auto cat(const char(&...strings)[Len]) {
    constexpr unsigned N = (... + Len) - sizeof...(Len);
    String<N + 1> result = {};
    result.c[N] = '\0';

    char* dst = result.c;
    for (const char* src : { strings... }) {
        for (; *src != '\0'; src++, dst++) {
            *dst = *src;
        }
    }
    return result;
}

//
//
//template <unsigned N>
//struct constexpr_string {
//    char value[N];
//};
//
//template<unsigned ...Len>
//constexpr auto cat(const char(&...strings)[Len]) {
//    constexpr unsigned N = (... + Len) - sizeof...(Len);
//    constexpr_string<N + 1> result = {};
//    result.value[N] = '\0';
//
//    char* dst = result.value;
//    for (const char* src : { strings... }) {
//        for (; *src != '\0'; src++, dst++) {
//            *dst = *src;
//        }
//    }
//    return result;
//}
//
//template<typename... T>
//struct type_name;
//
//template<typename T1, typename ... Other>
//struct type_name<T1, Other...> {
//    constexpr static auto get() {
//        return cat(type_name<T1>::get().value, ", ",
//            type_name<Other...>::get().value);
//    }
//};
//
//template<typename T>
//struct type_name<T> {
//    constexpr static auto get() {
//        if constexpr (std::is_pointer_v<T>) {
//            return cat(type_name<std::remove_pointer_t<T>>::get().value, "*");
//        }
//        else if constexpr (std::is_const_v<T>) {
//            return cat("const ", type_name<std::remove_const_t<T>>::get().value);
//        }
//        else if constexpr (std::is_rvalue_reference_v<T>) {
//            return cat(type_name<std::remove_reference_t<T>>::get().value, "&&");
//        }
//        else if constexpr (std::is_lvalue_reference_v<T>) {
//            return cat(type_name<std::remove_reference_t<T>>::get().value, "&");
//        }
//        else {
//            return constexpr_string<10>{"<unknown>"};
//        }
//    }
//};
//
//template<typename T>
//struct type_name<T[]> {
//    constexpr static auto get() {
//        return cat(type_name<T>::get().value, "[]");
//    }
//};
//
//template<typename T, std::size_t N>
//struct type_name<T[N]> {
//    constexpr static auto get() {
//        return cat(type_name<T>::get().value, "[]");
//    }
//};
//
//template<typename R, typename ... T>
//struct type_name<R(T...)> {
//    constexpr static auto get() {
//        return cat("(", type_name<T...>::get().value, ")",
//            " -> ", type_name<R>::get().value);
//    }
//};
//
//#define REGISTER_TYPE_NAME(__type__) \
//    template<> struct type_name<__type__> { \
//        constexpr static auto get() { \
//            return constexpr_string<sizeof #__type__>{#__type__}; \
//        } \
//    }
////
//REGISTER_TYPE_NAME(void);
////REGISTER_TYPE_NAME(int);
////REGISTER_TYPE_NAME(char);
////REGISTER_TYPE_NAME(long);
////REGISTER_TYPE_NAME(double);
////REGISTER_TYPE_NAME(float);
//
//double& f(const int&, char&&, float*, long[2][3]);
//double& g(const int&, char&&, float*, long[2][3], int (*)(char, char));