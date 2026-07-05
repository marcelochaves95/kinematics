#pragma once

#include <cmath>
#include <cstdio>
#include <functional>
#include <string>
#include <vector>

namespace kt {

struct Case {
    std::string name;
    std::function<void()> fn;
};

inline std::vector<Case>& registry() {
    static std::vector<Case> r;
    return r;
}

inline int& failures() {
    static int f = 0;
    return f;
}

struct Registrar {
    Registrar(const char* name, std::function<void()> fn) {
        registry().push_back({name, std::move(fn)});
    }
};

inline int run_all() {
    int passed = 0;
    for (auto& c : registry()) {
        int before = failures();
        c.fn();
        if (failures() == before) {
            ++passed;
        } else {
            std::printf("[FAILED] %s\n", c.name.c_str());
        }
    }
    std::printf("\n%d/%zu test cases passed, %d check(s) failed.\n",
                passed, registry().size(), failures());
    return failures() == 0 ? 0 : 1;
}

}

#define TEST_CASE(name)                                                        \
    static void name();                                                        \
    static kt::Registrar kt_reg_##name(#name, name);                           \
    static void name()

#define CHECK(cond)                                                            \
    do {                                                                       \
        if (!(cond)) {                                                         \
            ++kt::failures();                                                  \
            std::printf("  FAIL %s:%d  CHECK(%s)\n", __FILE__, __LINE__,       \
                        #cond);                                                \
        }                                                                      \
    } while (0)

#define CHECK_NEAR(a, b, eps)                                                  \
    do {                                                                       \
        double _d = std::fabs((double)(a) - (double)(b));                      \
        if (_d > (eps)) {                                                      \
            ++kt::failures();                                                  \
            std::printf("  FAIL %s:%d  |%s - %s| = %g > %g\n", __FILE__,       \
                        __LINE__, #a, #b, _d, (double)(eps));                  \
        }                                                                      \
    } while (0)
