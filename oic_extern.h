/* VBlank_oic_extern.h
    Helpers for declaring extern functions that belong to an OIC "namespace".

    The compiler recognizes function names that contain double-underscores (`__`) as
    encoding a namespaced external method. For example:
    
    extern void OCRuntime__PixelBindings__FillRect(int64_t x, int64_t y, int64_t w, int64_t h, int64_t color);

    will be interpreted by the `CCompiler` as a call to
    `OCRuntime.PixelBindings.FillRect`.

    These macros make it easier to declare such functions from C source.

    Usage examples:

    #include "VBlank_oic_extern.h"
    EXTERN_FN(OCRuntime__PixelBindings, void, FillRect, int64_t x, int64_t y, int64_t w, int64_t h, int64_t color);

    This expands to:
    extern void OCRuntime__PixelBindings__FillRect(int64_t x, int64_t y, int64_t w, int64_t h, int64_t color);
*/

#ifndef OIC_EXTERN_H
#define OIC_EXTERN_H

#include <stdint.h>
struct MODULE_METADATA MODULE_METADATA = { "MyModule", "1.0.0", "Your Name" };


/* Produce a symbol name by joining namespace and name with __ */
#define EXTERN_METHOD(ns, name) ns##__##name

/* Declare an extern function with a namespaced symbol.
- ns: namespace token(s) joined by __ (e.g. OCRuntime__PixelBindings)
- ret: return type (e.g. void)
- name: short method name (e.g. FillRect)
- ...: parameter list (e.g. int64_t x, int64_t y)

Example:
EXTERN_FN(OCRuntime__PixelBindings, void, FillRect, int64_t x, int64_t y, int64_t w, int64_t h, int64_t color);
*/
#define EXTERN_FN(ns, ret, name, ...) extern ret EXTERN_METHOD(ns, name)(__VA_ARGS__)


// Module metadata for output naming and info
struct MODULE_METADATA {
  const char* name;
  const char* version;
  const char* author;
};

EXTERN_FN(OCRuntime__PixelBindings, void, FillRect, int64_t x, int64_t y, int64_t w, int64_t h, int64_t color);
EXTERN_FN(OCRuntime__PixelBindings, void, SetPixel, int64_t x, int64_t y, int64_t color);
EXTERN_FN(OCRuntime__PixelBindings, void, Clear, int64_t color);
EXTERN_FN(OCRuntime__PixelBindings, void, RectFill, float x, float y, float w, float h, int64_t color);
EXTERN_FN(OCRuntime__PixelBindings, void, PlayOneShot, const char* soundName, float volume); 
EXTERN_FN(OCRuntime__PixelBindings, void, ButtonClick);
EXTERN_FN(OCRuntime__PixelBindings, void, SynthPlay, int64_t frequency, double duration);

struct Color {
    uint8_t r;
    uint8_t g;
    uint8_t b;
    uint8_t a;
};

#endif /* OIC_EXTERN_H */
