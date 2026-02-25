#include "oic_extern.h"

// Test extern function declaration
EXTERN_FN(OCRuntime__PixelBindings, void, FillRect, long x, long y, long w, long h, long color);

int main() {
    // Test function call
    FillRect(10, 20, 100, 50, 0xFF0000);
    
    return 0;
}
