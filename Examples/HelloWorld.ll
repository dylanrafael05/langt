; ModuleID = 'HelloWorld.lgt'
source_filename = "HelloWorld.lgt"

@str = private unnamed_addr constant [3 x i8] c"%d\00", align 1

define i1 @"<lgt>operator not(bool)"(i1 %0) {
.entry:
  %1 = xor i1 %0, true
  ret i1 %1
}

define i8 @"<lgt>operator unary -(i8)"(i8 %0) {
.entry:
  %1 = sub i8 0, %0
  ret i8 %1
}

define i16 @"<lgt>operator unary -(i16)"(i16 %0) {
.entry:
  %1 = sub i16 0, %0
  ret i16 %1
}

define i32 @"<lgt>operator unary -(i32)"(i32 %0) {
.entry:
  %1 = sub i32 0, %0
  ret i32 %1
}

define i64 @"<lgt>operator unary -(i64)"(i64 %0) {
.entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define i64 @"<lgt>operator unary -(isize)"(i64 %0) {
.entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define half @"<lgt>operator unary -(f16)"(half %0) {
.entry:
  %1 = fneg half %0
  ret half %1
}

define float @"<lgt>operator unary -(f32)"(float %0) {
.entry:
  %1 = fneg float %0
  ret float %1
}

define double @"<lgt>operator unary -(f64)"(double %0) {
.entry:
  %1 = fneg double %0
  ret double %1
}

define i16 @"<lgt>operator unary -(u8)"(i8 %0) {
.entry:
  %1 = zext i8 %0 to i16
  %2 = sub i16 0, %1
  ret i16 %2
}

define i32 @"<lgt>operator unary -(u16)"(i16 %0) {
.entry:
  %1 = zext i16 %0 to i32
  %2 = sub i32 0, %1
  ret i32 %2
}

define i64 @"<lgt>operator unary -(u32)"(i32 %0) {
.entry:
  %1 = zext i32 %0 to i64
  %2 = sub i64 0, %1
  ret i64 %2
}

define half @"<lgt>operator +(u8, f16)"(i8 %0, half %1) {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, u8)"(half %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(u8, f32)"(i8 %0, float %1) {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, u8)"(float %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(u8, f64)"(i8 %0, double %1) {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, u8)"(double %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(u16, f16)"(i16 %0, half %1) {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, u16)"(half %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(u16, f32)"(i16 %0, float %1) {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, u16)"(float %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(u16, f64)"(i16 %0, double %1) {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, u16)"(double %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(u32, f16)"(i32 %0, half %1) {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, u32)"(half %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(u32, f32)"(i32 %0, float %1) {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, u32)"(float %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(u32, f64)"(i32 %0, double %1) {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, u32)"(double %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(u64, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, u64)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(u64, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, u64)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(u64, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, u64)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(usize, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, usize)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(usize, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, usize)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(usize, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, usize)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(i8, f16)"(i8 %0, half %1) {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, i8)"(half %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(i8, f32)"(i8 %0, float %1) {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, i8)"(float %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(i8, f64)"(i8 %0, double %1) {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, i8)"(double %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(i16, f16)"(i16 %0, half %1) {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, i16)"(half %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(i16, f32)"(i16 %0, float %1) {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, i16)"(float %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(i16, f64)"(i16 %0, double %1) {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, i16)"(double %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(i32, f16)"(i32 %0, half %1) {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, i32)"(half %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(i32, f32)"(i32 %0, float %1) {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, i32)"(float %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(i32, f64)"(i32 %0, double %1) {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, i32)"(double %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(i64, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, i64)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(i64, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, i64)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(i64, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, i64)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define half @"<lgt>operator +(isize, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(f16, isize)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fadd half %0, %2
  ret half %3
}

define float @"<lgt>operator +(isize, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, isize)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(isize, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, isize)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define i8 @"<lgt>operator +(i8, i8)"(i8 %0, i8 %1) {
.entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator +(i8, i16)"(i8 %0, i16 %1) {
.entry:
  %2 = sext i8 %0 to i16
  %3 = add i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator +(i16, i8)"(i16 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i16
  %3 = add i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator +(i8, i32)"(i8 %0, i32 %1) {
.entry:
  %2 = sext i8 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(i32, i8)"(i32 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i32
  %3 = add i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator +(i8, i64)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(i64, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(i8, isize)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(isize, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator +(i16, i16)"(i16 %0, i16 %1) {
.entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator +(i16, i32)"(i16 %0, i32 %1) {
.entry:
  %2 = sext i16 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(i32, i16)"(i32 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i32
  %3 = add i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator +(i16, i64)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(i64, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(i16, isize)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(isize, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator +(i32, i32)"(i32 %0, i32 %1) {
.entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator +(i32, i64)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(i64, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(i32, isize)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(isize, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(i64, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(i64, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(isize, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(isize, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i8 @"<lgt>operator +(u8, u8)"(i8 %0, i8 %1) {
.entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator +(u8, u16)"(i8 %0, i16 %1) {
.entry:
  %2 = zext i8 %0 to i16
  %3 = add i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator +(u16, u8)"(i16 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i16
  %3 = add i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator +(u8, u32)"(i8 %0, i32 %1) {
.entry:
  %2 = zext i8 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(u32, u8)"(i32 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i32
  %3 = add i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator +(u8, u64)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(u64, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(u8, usize)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(usize, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator +(u16, u16)"(i16 %0, i16 %1) {
.entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator +(u16, u32)"(i16 %0, i32 %1) {
.entry:
  %2 = zext i16 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(u32, u16)"(i32 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i32
  %3 = add i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator +(u16, u64)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(u64, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(u16, usize)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(usize, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator +(u32, u32)"(i32 %0, i32 %1) {
.entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator +(u32, u64)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(u64, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(u32, usize)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(usize, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = add i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator +(u64, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(u64, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(usize, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator +(usize, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator +(f16, f16)"(half %0, half %1) {
.entry:
  %2 = fadd half %0, %1
  ret half %2
}

define float @"<lgt>operator +(f16, f32)"(half %0, float %1) {
.entry:
  %2 = fpext half %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(f32, f16)"(float %0, half %1) {
.entry:
  %2 = fpext half %1 to float
  %3 = fadd float %0, %2
  ret float %3
}

define double @"<lgt>operator +(f16, f64)"(half %0, double %1) {
.entry:
  %2 = fpext half %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, f16)"(double %0, half %1) {
.entry:
  %2 = fpext half %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define float @"<lgt>operator +(f32, f32)"(float %0, float %1) {
.entry:
  %2 = fadd float %0, %1
  ret float %2
}

define double @"<lgt>operator +(f32, f64)"(float %0, double %1) {
.entry:
  %2 = fpext float %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(f64, f32)"(double %0, float %1) {
.entry:
  %2 = fpext float %1 to double
  %3 = fadd double %0, %2
  ret double %3
}

define double @"<lgt>operator +(f64, f64)"(double %0, double %1) {
.entry:
  %2 = fadd double %0, %1
  ret double %2
}

define half @"<lgt>operator -(u8, f16)"(i8 %0, half %1) {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, u8)"(half %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(u8, f32)"(i8 %0, float %1) {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, u8)"(float %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(u8, f64)"(i8 %0, double %1) {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, u8)"(double %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(u16, f16)"(i16 %0, half %1) {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, u16)"(half %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(u16, f32)"(i16 %0, float %1) {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, u16)"(float %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(u16, f64)"(i16 %0, double %1) {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, u16)"(double %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(u32, f16)"(i32 %0, half %1) {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, u32)"(half %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(u32, f32)"(i32 %0, float %1) {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, u32)"(float %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(u32, f64)"(i32 %0, double %1) {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, u32)"(double %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(u64, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, u64)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(u64, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, u64)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(u64, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, u64)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(usize, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, usize)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(usize, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, usize)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(usize, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, usize)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(i8, f16)"(i8 %0, half %1) {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, i8)"(half %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(i8, f32)"(i8 %0, float %1) {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, i8)"(float %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(i8, f64)"(i8 %0, double %1) {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, i8)"(double %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(i16, f16)"(i16 %0, half %1) {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, i16)"(half %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(i16, f32)"(i16 %0, float %1) {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, i16)"(float %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(i16, f64)"(i16 %0, double %1) {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, i16)"(double %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(i32, f16)"(i32 %0, half %1) {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, i32)"(half %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(i32, f32)"(i32 %0, float %1) {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, i32)"(float %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(i32, f64)"(i32 %0, double %1) {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, i32)"(double %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(i64, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, i64)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(i64, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, i64)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(i64, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, i64)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(isize, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(f16, isize)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(isize, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, isize)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(isize, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, isize)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define i8 @"<lgt>operator -(i8, i8)"(i8 %0, i8 %1) {
.entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator -(i8, i16)"(i8 %0, i16 %1) {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sub i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator -(i16, i8)"(i16 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sub i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator -(i8, i32)"(i8 %0, i32 %1) {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(i32, i8)"(i32 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(i8, i64)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(i64, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(i8, isize)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(isize, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator -(i16, i16)"(i16 %0, i16 %1) {
.entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator -(i16, i32)"(i16 %0, i32 %1) {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(i32, i16)"(i32 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(i16, i64)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(i64, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(i16, isize)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(isize, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator -(i32, i32)"(i32 %0, i32 %1) {
.entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator -(i32, i64)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(i64, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(i32, isize)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(isize, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(i64, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(i64, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(isize, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(isize, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i8 @"<lgt>operator -(u8, u8)"(i8 %0, i8 %1) {
.entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator -(u8, u16)"(i8 %0, i16 %1) {
.entry:
  %2 = zext i8 %0 to i16
  %3 = sub i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator -(u16, u8)"(i16 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i16
  %3 = sub i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator -(u8, u32)"(i8 %0, i32 %1) {
.entry:
  %2 = zext i8 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(u32, u8)"(i32 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(u8, u64)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(u64, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(u8, usize)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(usize, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator -(u16, u16)"(i16 %0, i16 %1) {
.entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator -(u16, u32)"(i16 %0, i32 %1) {
.entry:
  %2 = zext i16 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(u32, u16)"(i32 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(u16, u64)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(u64, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(u16, usize)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(usize, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator -(u32, u32)"(i32 %0, i32 %1) {
.entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator -(u32, u64)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(u64, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(u32, usize)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(usize, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(u64, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(u64, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(usize, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator -(usize, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator -(f16, f16)"(half %0, half %1) {
.entry:
  %2 = fsub half %0, %1
  ret half %2
}

define float @"<lgt>operator -(f16, f32)"(half %0, float %1) {
.entry:
  %2 = fpext half %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(f32, f16)"(float %0, half %1) {
.entry:
  %2 = fpext half %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(f16, f64)"(half %0, double %1) {
.entry:
  %2 = fpext half %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, f16)"(double %0, half %1) {
.entry:
  %2 = fpext half %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define float @"<lgt>operator -(f32, f32)"(float %0, float %1) {
.entry:
  %2 = fsub float %0, %1
  ret float %2
}

define double @"<lgt>operator -(f32, f64)"(float %0, double %1) {
.entry:
  %2 = fpext float %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(f64, f32)"(double %0, float %1) {
.entry:
  %2 = fpext float %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define double @"<lgt>operator -(f64, f64)"(double %0, double %1) {
.entry:
  %2 = fsub double %0, %1
  ret double %2
}

define half @"<lgt>operator *(u8, f16)"(i8 %0, half %1) {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, u8)"(half %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(u8, f32)"(i8 %0, float %1) {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, u8)"(float %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(u8, f64)"(i8 %0, double %1) {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, u8)"(double %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(u16, f16)"(i16 %0, half %1) {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, u16)"(half %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(u16, f32)"(i16 %0, float %1) {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, u16)"(float %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(u16, f64)"(i16 %0, double %1) {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, u16)"(double %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(u32, f16)"(i32 %0, half %1) {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, u32)"(half %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(u32, f32)"(i32 %0, float %1) {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, u32)"(float %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(u32, f64)"(i32 %0, double %1) {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, u32)"(double %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(u64, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, u64)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(u64, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, u64)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(u64, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, u64)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(usize, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, usize)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(usize, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, usize)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(usize, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, usize)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(i8, f16)"(i8 %0, half %1) {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, i8)"(half %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(i8, f32)"(i8 %0, float %1) {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, i8)"(float %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(i8, f64)"(i8 %0, double %1) {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, i8)"(double %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(i16, f16)"(i16 %0, half %1) {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, i16)"(half %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(i16, f32)"(i16 %0, float %1) {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, i16)"(float %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(i16, f64)"(i16 %0, double %1) {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, i16)"(double %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(i32, f16)"(i32 %0, half %1) {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, i32)"(half %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(i32, f32)"(i32 %0, float %1) {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, i32)"(float %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(i32, f64)"(i32 %0, double %1) {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, i32)"(double %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(i64, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, i64)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(i64, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, i64)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(i64, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, i64)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define half @"<lgt>operator *(isize, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(f16, isize)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fmul half %0, %2
  ret half %3
}

define float @"<lgt>operator *(isize, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, isize)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(isize, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, isize)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define i8 @"<lgt>operator *(i8, i8)"(i8 %0, i8 %1) {
.entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator *(i8, i16)"(i8 %0, i16 %1) {
.entry:
  %2 = sext i8 %0 to i16
  %3 = mul i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator *(i16, i8)"(i16 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i16
  %3 = mul i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator *(i8, i32)"(i8 %0, i32 %1) {
.entry:
  %2 = sext i8 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(i32, i8)"(i32 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i32
  %3 = mul i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator *(i8, i64)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(i64, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(i8, isize)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(isize, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator *(i16, i16)"(i16 %0, i16 %1) {
.entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator *(i16, i32)"(i16 %0, i32 %1) {
.entry:
  %2 = sext i16 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(i32, i16)"(i32 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i32
  %3 = mul i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator *(i16, i64)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(i64, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(i16, isize)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(isize, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator *(i32, i32)"(i32 %0, i32 %1) {
.entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator *(i32, i64)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(i64, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(i32, isize)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(isize, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(i64, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(i64, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(isize, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(isize, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i8 @"<lgt>operator *(u8, u8)"(i8 %0, i8 %1) {
.entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator *(u8, u16)"(i8 %0, i16 %1) {
.entry:
  %2 = zext i8 %0 to i16
  %3 = mul i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator *(u16, u8)"(i16 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i16
  %3 = mul i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator *(u8, u32)"(i8 %0, i32 %1) {
.entry:
  %2 = zext i8 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(u32, u8)"(i32 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i32
  %3 = mul i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator *(u8, u64)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(u64, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(u8, usize)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(usize, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator *(u16, u16)"(i16 %0, i16 %1) {
.entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator *(u16, u32)"(i16 %0, i32 %1) {
.entry:
  %2 = zext i16 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(u32, u16)"(i32 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i32
  %3 = mul i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator *(u16, u64)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(u64, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(u16, usize)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(usize, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator *(u32, u32)"(i32 %0, i32 %1) {
.entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator *(u32, u64)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(u64, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(u32, usize)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(usize, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = mul i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator *(u64, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(u64, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(usize, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator *(usize, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator *(f16, f16)"(half %0, half %1) {
.entry:
  %2 = fmul half %0, %1
  ret half %2
}

define float @"<lgt>operator *(f16, f32)"(half %0, float %1) {
.entry:
  %2 = fpext half %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(f32, f16)"(float %0, half %1) {
.entry:
  %2 = fpext half %1 to float
  %3 = fmul float %0, %2
  ret float %3
}

define double @"<lgt>operator *(f16, f64)"(half %0, double %1) {
.entry:
  %2 = fpext half %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, f16)"(double %0, half %1) {
.entry:
  %2 = fpext half %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define float @"<lgt>operator *(f32, f32)"(float %0, float %1) {
.entry:
  %2 = fmul float %0, %1
  ret float %2
}

define double @"<lgt>operator *(f32, f64)"(float %0, double %1) {
.entry:
  %2 = fpext float %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(f64, f32)"(double %0, float %1) {
.entry:
  %2 = fpext float %1 to double
  %3 = fmul double %0, %2
  ret double %3
}

define double @"<lgt>operator *(f64, f64)"(double %0, double %1) {
.entry:
  %2 = fmul double %0, %1
  ret double %2
}

define half @"<lgt>operator /(u8, f16)"(i8 %0, half %1) {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, u8)"(half %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(u8, f32)"(i8 %0, float %1) {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, u8)"(float %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(u8, f64)"(i8 %0, double %1) {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, u8)"(double %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(u16, f16)"(i16 %0, half %1) {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, u16)"(half %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(u16, f32)"(i16 %0, float %1) {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, u16)"(float %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(u16, f64)"(i16 %0, double %1) {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, u16)"(double %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(u32, f16)"(i32 %0, half %1) {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, u32)"(half %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(u32, f32)"(i32 %0, float %1) {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, u32)"(float %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(u32, f64)"(i32 %0, double %1) {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, u32)"(double %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(u64, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, u64)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(u64, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, u64)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(u64, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, u64)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(usize, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, usize)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(usize, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, usize)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(usize, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, usize)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(i8, f16)"(i8 %0, half %1) {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, i8)"(half %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(i8, f32)"(i8 %0, float %1) {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, i8)"(float %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(i8, f64)"(i8 %0, double %1) {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, i8)"(double %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(i16, f16)"(i16 %0, half %1) {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, i16)"(half %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(i16, f32)"(i16 %0, float %1) {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, i16)"(float %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(i16, f64)"(i16 %0, double %1) {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, i16)"(double %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(i32, f16)"(i32 %0, half %1) {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, i32)"(half %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(i32, f32)"(i32 %0, float %1) {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, i32)"(float %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(i32, f64)"(i32 %0, double %1) {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, i32)"(double %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(i64, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, i64)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(i64, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, i64)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(i64, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, i64)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(isize, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(f16, isize)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(isize, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, isize)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(isize, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, isize)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define i8 @"<lgt>operator /(i8, i8)"(i8 %0, i8 %1) {
.entry:
  %2 = sdiv i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator /(i8, i16)"(i8 %0, i16 %1) {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sdiv i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator /(i16, i8)"(i16 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sdiv i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator /(i8, i32)"(i8 %0, i32 %1) {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(i32, i8)"(i32 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(i8, i64)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(i64, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(i8, isize)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(isize, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator /(i16, i16)"(i16 %0, i16 %1) {
.entry:
  %2 = sdiv i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator /(i16, i32)"(i16 %0, i32 %1) {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(i32, i16)"(i32 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(i16, i64)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(i64, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(i16, isize)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(isize, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator /(i32, i32)"(i32 %0, i32 %1) {
.entry:
  %2 = sdiv i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator /(i32, i64)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(i64, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(i32, isize)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(isize, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(i64, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(i64, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(isize, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(isize, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i8 @"<lgt>operator /(u8, u8)"(i8 %0, i8 %1) {
.entry:
  %2 = udiv i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator /(u8, u16)"(i8 %0, i16 %1) {
.entry:
  %2 = zext i8 %0 to i16
  %3 = udiv i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator /(u16, u8)"(i16 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i16
  %3 = udiv i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator /(u8, u32)"(i8 %0, i32 %1) {
.entry:
  %2 = zext i8 %0 to i32
  %3 = udiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(u32, u8)"(i32 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i32
  %3 = udiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(u8, u64)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(u64, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(u8, usize)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(usize, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator /(u16, u16)"(i16 %0, i16 %1) {
.entry:
  %2 = udiv i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator /(u16, u32)"(i16 %0, i32 %1) {
.entry:
  %2 = zext i16 %0 to i32
  %3 = udiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(u32, u16)"(i32 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i32
  %3 = udiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(u16, u64)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(u64, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(u16, usize)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(usize, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator /(u32, u32)"(i32 %0, i32 %1) {
.entry:
  %2 = udiv i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator /(u32, u64)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(u64, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(u32, usize)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(usize, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(u64, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(u64, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(usize, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator /(usize, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator /(f16, f16)"(half %0, half %1) {
.entry:
  %2 = fdiv half %0, %1
  ret half %2
}

define float @"<lgt>operator /(f16, f32)"(half %0, float %1) {
.entry:
  %2 = fpext half %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(f32, f16)"(float %0, half %1) {
.entry:
  %2 = fpext half %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(f16, f64)"(half %0, double %1) {
.entry:
  %2 = fpext half %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, f16)"(double %0, half %1) {
.entry:
  %2 = fpext half %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define float @"<lgt>operator /(f32, f32)"(float %0, float %1) {
.entry:
  %2 = fdiv float %0, %1
  ret float %2
}

define double @"<lgt>operator /(f32, f64)"(float %0, double %1) {
.entry:
  %2 = fpext float %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(f64, f32)"(double %0, float %1) {
.entry:
  %2 = fpext float %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define double @"<lgt>operator /(f64, f64)"(double %0, double %1) {
.entry:
  %2 = fdiv double %0, %1
  ret double %2
}

define half @"<lgt>operator %(u8, f16)"(i8 %0, half %1) {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, u8)"(half %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(u8, f32)"(i8 %0, float %1) {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, u8)"(float %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(u8, f64)"(i8 %0, double %1) {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, u8)"(double %0, i8 %1) {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(u16, f16)"(i16 %0, half %1) {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, u16)"(half %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(u16, f32)"(i16 %0, float %1) {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, u16)"(float %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(u16, f64)"(i16 %0, double %1) {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, u16)"(double %0, i16 %1) {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(u32, f16)"(i32 %0, half %1) {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, u32)"(half %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(u32, f32)"(i32 %0, float %1) {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, u32)"(float %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(u32, f64)"(i32 %0, double %1) {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, u32)"(double %0, i32 %1) {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(u64, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, u64)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(u64, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, u64)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(u64, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, u64)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(usize, f16)"(i64 %0, half %1) {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, usize)"(half %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(usize, f32)"(i64 %0, float %1) {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, usize)"(float %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(usize, f64)"(i64 %0, double %1) {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, usize)"(double %0, i64 %1) {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(i8, f16)"(i8 %0, half %1) {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, i8)"(half %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(i8, f32)"(i8 %0, float %1) {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, i8)"(float %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(i8, f64)"(i8 %0, double %1) {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, i8)"(double %0, i8 %1) {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(i16, f16)"(i16 %0, half %1) {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, i16)"(half %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(i16, f32)"(i16 %0, float %1) {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, i16)"(float %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(i16, f64)"(i16 %0, double %1) {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, i16)"(double %0, i16 %1) {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(i32, f16)"(i32 %0, half %1) {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, i32)"(half %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(i32, f32)"(i32 %0, float %1) {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, i32)"(float %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(i32, f64)"(i32 %0, double %1) {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, i32)"(double %0, i32 %1) {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(i64, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, i64)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(i64, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, i64)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(i64, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, i64)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(isize, f16)"(i64 %0, half %1) {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(f16, isize)"(half %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(isize, f32)"(i64 %0, float %1) {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, isize)"(float %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(isize, f64)"(i64 %0, double %1) {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, isize)"(double %0, i64 %1) {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define i8 @"<lgt>operator %(i8, i8)"(i8 %0, i8 %1) {
.entry:
  %2 = srem i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator %(i8, i16)"(i8 %0, i16 %1) {
.entry:
  %2 = sext i8 %0 to i16
  %3 = srem i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator %(i16, i8)"(i16 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i16
  %3 = srem i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator %(i8, i32)"(i8 %0, i32 %1) {
.entry:
  %2 = sext i8 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(i32, i8)"(i32 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(i8, i64)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(i64, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(i8, isize)"(i8 %0, i64 %1) {
.entry:
  %2 = sext i8 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(isize, i8)"(i64 %0, i8 %1) {
.entry:
  %2 = sext i8 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator %(i16, i16)"(i16 %0, i16 %1) {
.entry:
  %2 = srem i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator %(i16, i32)"(i16 %0, i32 %1) {
.entry:
  %2 = sext i16 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(i32, i16)"(i32 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(i16, i64)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(i64, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(i16, isize)"(i16 %0, i64 %1) {
.entry:
  %2 = sext i16 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(isize, i16)"(i64 %0, i16 %1) {
.entry:
  %2 = sext i16 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator %(i32, i32)"(i32 %0, i32 %1) {
.entry:
  %2 = srem i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator %(i32, i64)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(i64, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(i32, isize)"(i32 %0, i64 %1) {
.entry:
  %2 = sext i32 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(isize, i32)"(i64 %0, i32 %1) {
.entry:
  %2 = sext i32 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(i64, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(i64, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(isize, i64)"(i64 %0, i64 %1) {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(isize, isize)"(i64 %0, i64 %1) {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i8 @"<lgt>operator %(u8, u8)"(i8 %0, i8 %1) {
.entry:
  %2 = urem i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator %(u8, u16)"(i8 %0, i16 %1) {
.entry:
  %2 = zext i8 %0 to i16
  %3 = urem i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator %(u16, u8)"(i16 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i16
  %3 = urem i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator %(u8, u32)"(i8 %0, i32 %1) {
.entry:
  %2 = zext i8 %0 to i32
  %3 = urem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(u32, u8)"(i32 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i32
  %3 = urem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(u8, u64)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(u64, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(u8, usize)"(i8 %0, i64 %1) {
.entry:
  %2 = zext i8 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(usize, u8)"(i64 %0, i8 %1) {
.entry:
  %2 = zext i8 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator %(u16, u16)"(i16 %0, i16 %1) {
.entry:
  %2 = urem i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator %(u16, u32)"(i16 %0, i32 %1) {
.entry:
  %2 = zext i16 %0 to i32
  %3 = urem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(u32, u16)"(i32 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i32
  %3 = urem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(u16, u64)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(u64, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(u16, usize)"(i16 %0, i64 %1) {
.entry:
  %2 = zext i16 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(usize, u16)"(i64 %0, i16 %1) {
.entry:
  %2 = zext i16 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator %(u32, u32)"(i32 %0, i32 %1) {
.entry:
  %2 = urem i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator %(u32, u64)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(u64, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(u32, usize)"(i32 %0, i64 %1) {
.entry:
  %2 = zext i32 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(usize, u32)"(i64 %0, i32 %1) {
.entry:
  %2 = zext i32 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(u64, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(u64, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(usize, u64)"(i64 %0, i64 %1) {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @"<lgt>operator %(usize, usize)"(i64 %0, i64 %1) {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator %(f16, f16)"(half %0, half %1) {
.entry:
  %2 = frem half %0, %1
  ret half %2
}

define float @"<lgt>operator %(f16, f32)"(half %0, float %1) {
.entry:
  %2 = fpext half %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(f32, f16)"(float %0, half %1) {
.entry:
  %2 = fpext half %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(f16, f64)"(half %0, double %1) {
.entry:
  %2 = fpext half %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, f16)"(double %0, half %1) {
.entry:
  %2 = fpext half %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define float @"<lgt>operator %(f32, f32)"(float %0, float %1) {
.entry:
  %2 = frem float %0, %1
  ret float %2
}

define double @"<lgt>operator %(f32, f64)"(float %0, double %1) {
.entry:
  %2 = fpext float %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(f64, f32)"(double %0, float %1) {
.entry:
  %2 = fpext float %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define double @"<lgt>operator %(f64, f64)"(double %0, double %1) {
.entry:
  %2 = frem double %0, %1
  ret double %2
}

define void @"<lgt>main()"() {
.entry:
  call void (ptr, ...) @printf(ptr @str, i64 8)
  ret void
}

declare void @printf(ptr, ...)
