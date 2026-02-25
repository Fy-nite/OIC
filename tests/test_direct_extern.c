// Test with direct extern declaration (no macros)
extern void OCRuntime__PixelBindings__FillRect(long x, long y, long w, long h, long color);

int main() {
    FillRect(10, 20, 100, 50, 0xFF0000);
    return 0;
}
