// Test compilation with warnings and errors

extern void OCRuntime__PixelBindings__FillRect(long x, long y, long w, long h, long color);

void test_function(int x, int y) {
    int unused_var = 42;  // Should warn about unused variable
    
    FillRect(10, 20, 100, 50, 255);  // Should warn about assumed int32
}

int main() {
    test_function(1, 2);
    return 0;
}
