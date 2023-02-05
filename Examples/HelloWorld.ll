; ModuleID = 'HelloWorld.lgt'
source_filename = "HelloWorld.lgt"

%_LG1X7Wrapper1X22G1X7Wrapper1X8PS1X3u32 = type { %_LG1X7Wrapper1X8PS1X3u32 }
%_LG1X7Wrapper1X8PS1X3u32 = type { ptr }
%_LG1X4Pair2X7S1X3i3221G1X7Wrapper1X7S1X3u32 = type { i32, %_LG1X7Wrapper1X7S1X3u32 }
%_LG1X7Wrapper1X7S1X3u32 = type { i32 }

@str = private unnamed_addr constant [3 x i8] c"%d\00", align 1

define i1 @_L1X6op_not1S1X4bool(i1 %0) local_unnamed_addr {
.entry:
  %1 = xor i1 %0, true
  ret i1 %1
}

define i8 @_L1X6op_neg1S1X2i8(i8 %0) local_unnamed_addr {
.entry:
  %1 = sub i8 0, %0
  ret i8 %1
}

define i16 @_L1X6op_neg1S1X3i16(i16 %0) local_unnamed_addr {
.entry:
  %1 = sub i16 0, %0
  ret i16 %1
}

define i32 @_L1X6op_neg1S1X3i32(i32 %0) local_unnamed_addr {
.entry:
  %1 = sub i32 0, %0
  ret i32 %1
}

define i64 @_L1X6op_neg1S1X3i64(i64 %0) local_unnamed_addr {
.entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define i64 @_L1X6op_neg1S1X5isize(i64 %0) local_unnamed_addr {
.entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define half @_L1X6op_neg1S1X3f16(half %0) local_unnamed_addr {
.entry:
  %1 = fneg half %0
  ret half %1
}

define float @_L1X6op_neg1S1X3f32(float %0) local_unnamed_addr {
.entry:
  %1 = fneg float %0
  ret float %1
}

define double @_L1X6op_neg1S1X3f64(double %0) local_unnamed_addr {
.entry:
  %1 = fneg double %0
  ret double %1
}

define i16 @_L1X6op_neg1S1X2u8(i8 %0) local_unnamed_addr {
.entry:
  %1 = zext i8 %0 to i16
  %2 = sub nsw i16 0, %1
  ret i16 %2
}

define i32 @_L1X6op_neg1S1X3u16(i16 %0) local_unnamed_addr {
.entry:
  %1 = zext i16 %0 to i32
  %2 = sub nsw i32 0, %1
  ret i32 %2
}

define i64 @_L1X6op_neg1S1X3u32(i32 %0) local_unnamed_addr {
.entry:
  %1 = zext i32 %0 to i64
  %2 = sub nsw i64 0, %1
  ret i64 %2
}

define half @_L1X6op_add2S1X2u8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X2u8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X2u8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X2u8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X2u8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X2u8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3u16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3u16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3u16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3u16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3u16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3u16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3u32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3u32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3u32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3u32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3u32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3u32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3u64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3u64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3u64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3u64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3u64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3u64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X5usizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X5usize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X5usizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X5usize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X5usizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X5usize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X2i8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X2i8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X2i8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X2i8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X2i8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X2i8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3i16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3i16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3i16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3i16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3i16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3i16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3i32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3i32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3i32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3i32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3i32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3i32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X3i64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X3i64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X3i64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3i64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3i64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3i64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @_L1X6op_add2S1X5isizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @_L1X6op_add2S1X3f16S1X5isize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @_L1X6op_add2S1X5isizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X5isize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X5isizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X5isize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define i8 @_L1X6op_add2S1X2i8S1X2i8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_add2S1X2i8S1X3i16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = add i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_add2S1X3i16S1X2i8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = add i16 %2, %0
  ret i16 %3
}

define i32 @_L1X6op_add2S1X2i8S1X3i32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_add2S1X3i32S1X2i8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_add2S1X2i8S1X3i64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i64S1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X2i8S1X5isize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5isizeS1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i16 @_L1X6op_add2S1X3i16S1X3i16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_add2S1X3i16S1X3i32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_add2S1X3i32S1X3i16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_add2S1X3i16S1X3i64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i64S1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i16S1X5isize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5isizeS1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i32 @_L1X6op_add2S1X3i32S1X3i32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_add2S1X3i32S1X3i64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i64S1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i32S1X5isize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5isizeS1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3i64S1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X3i64S1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X5isizeS1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X5isizeS1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i8 @_L1X6op_add2S1X2u8S1X2u8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_add2S1X2u8S1X3u16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i16
  %3 = add i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_add2S1X3u16S1X2u8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i16
  %3 = add i16 %2, %0
  ret i16 %3
}

define i32 @_L1X6op_add2S1X2u8S1X3u32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_add2S1X3u32S1X2u8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_add2S1X2u8S1X3u64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u64S1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X2u8S1X5usize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5usizeS1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i16 @_L1X6op_add2S1X3u16S1X3u16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_add2S1X3u16S1X3u32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_add2S1X3u32S1X3u16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_add2S1X3u16S1X3u64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u64S1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u16S1X5usize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5usizeS1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i32 @_L1X6op_add2S1X3u32S1X3u32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_add2S1X3u32S1X3u64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u64S1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u32S1X5usize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_add2S1X5usizeS1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_add2S1X3u64S1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X3u64S1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X5usizeS1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_add2S1X5usizeS1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define half @_L1X6op_add2S1X3f16S1X3f16(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fadd half %0, %1
  ret half %2
}

define float @_L1X6op_add2S1X3f16S1X3f32(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @_L1X6op_add2S1X3f32S1X3f16(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @_L1X6op_add2S1X3f16S1X3f64(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3f16(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define float @_L1X6op_add2S1X3f32S1X3f32(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fadd float %0, %1
  ret float %2
}

define double @_L1X6op_add2S1X3f32S1X3f64(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3f32(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define double @_L1X6op_add2S1X3f64S1X3f64(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fadd double %0, %1
  ret double %2
}

define half @_L1X6op_sub2S1X2u8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X2u8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X2u8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X2u8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X2u8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X2u8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3u16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3u16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3u16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3u16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3u16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3u16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3u32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3u32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3u32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3u32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3u32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3u32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3u64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3u64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3u64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3u64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3u64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3u64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X5usizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X5usize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X5usizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X5usize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X5usizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X5usize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X2i8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X2i8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X2i8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X2i8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X2i8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X2i8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3i16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3i16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3i16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3i16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3i16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3i16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3i32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3i32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3i32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3i32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3i32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3i32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X3i64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X3i64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X3i64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3i64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3i64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3i64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @_L1X6op_sub2S1X5isizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @_L1X6op_sub2S1X3f16S1X5isize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @_L1X6op_sub2S1X5isizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X5isize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X5isizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X5isize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define i8 @_L1X6op_sub2S1X2i8S1X2i8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_sub2S1X2i8S1X3i16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sub i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_sub2S1X3i16S1X2i8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sub i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_sub2S1X2i8S1X3i32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_sub2S1X3i32S1X2i8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_sub2S1X2i8S1X3i64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i64S1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X2i8S1X5isize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5isizeS1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_sub2S1X3i16S1X3i16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_sub2S1X3i16S1X3i32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_sub2S1X3i32S1X3i16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_sub2S1X3i16S1X3i64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i64S1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i16S1X5isize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5isizeS1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_sub2S1X3i32S1X3i32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_sub2S1X3i32S1X3i64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i64S1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i32S1X5isize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5isizeS1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3i64S1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X3i64S1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X5isizeS1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X5isizeS1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i8 @_L1X6op_sub2S1X2u8S1X2u8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_sub2S1X2u8S1X3u16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i16
  %3 = sub i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_sub2S1X3u16S1X2u8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i16
  %3 = sub i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_sub2S1X2u8S1X3u32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_sub2S1X3u32S1X2u8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_sub2S1X2u8S1X3u64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u64S1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X2u8S1X5usize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5usizeS1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_sub2S1X3u16S1X3u16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_sub2S1X3u16S1X3u32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_sub2S1X3u32S1X3u16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_sub2S1X3u16S1X3u64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u64S1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u16S1X5usize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5usizeS1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_sub2S1X3u32S1X3u32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_sub2S1X3u32S1X3u64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u64S1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u32S1X5usize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X5usizeS1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_sub2S1X3u64S1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X3u64S1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X5usizeS1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_sub2S1X5usizeS1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define half @_L1X6op_sub2S1X3f16S1X3f16(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fsub half %0, %1
  ret half %2
}

define float @_L1X6op_sub2S1X3f16S1X3f32(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @_L1X6op_sub2S1X3f32S1X3f16(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @_L1X6op_sub2S1X3f16S1X3f64(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3f16(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define float @_L1X6op_sub2S1X3f32S1X3f32(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fsub float %0, %1
  ret float %2
}

define double @_L1X6op_sub2S1X3f32S1X3f64(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3f32(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define double @_L1X6op_sub2S1X3f64S1X3f64(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fsub double %0, %1
  ret double %2
}

define half @_L1X6op_mul2S1X2u8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X2u8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X2u8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X2u8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X2u8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X2u8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3u16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3u16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3u16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3u16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3u16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3u16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3u32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3u32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3u32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3u32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3u32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3u32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3u64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3u64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3u64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3u64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3u64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3u64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X5usizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X5usize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X5usizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X5usize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X5usizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X5usize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X2i8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X2i8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X2i8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X2i8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X2i8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X2i8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3i16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3i16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3i16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3i16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3i16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3i16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3i32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3i32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3i32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3i32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3i32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3i32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X3i64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X3i64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X3i64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3i64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3i64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3i64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @_L1X6op_mul2S1X5isizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @_L1X6op_mul2S1X3f16S1X5isize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @_L1X6op_mul2S1X5isizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X5isize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X5isizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X5isize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define i8 @_L1X6op_mul2S1X2i8S1X2i8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_mul2S1X2i8S1X3i16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = mul i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_mul2S1X3i16S1X2i8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = mul i16 %2, %0
  ret i16 %3
}

define i32 @_L1X6op_mul2S1X2i8S1X3i32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mul2S1X3i32S1X2i8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_mul2S1X2i8S1X3i64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i64S1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X2i8S1X5isize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5isizeS1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i16 @_L1X6op_mul2S1X3i16S1X3i16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_mul2S1X3i16S1X3i32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mul2S1X3i32S1X3i16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_mul2S1X3i16S1X3i64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i64S1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i16S1X5isize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5isizeS1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i32 @_L1X6op_mul2S1X3i32S1X3i32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_mul2S1X3i32S1X3i64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i64S1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i32S1X5isize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5isizeS1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3i64S1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X3i64S1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X5isizeS1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X5isizeS1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i8 @_L1X6op_mul2S1X2u8S1X2u8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_mul2S1X2u8S1X3u16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i16
  %3 = mul i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_mul2S1X3u16S1X2u8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i16
  %3 = mul i16 %2, %0
  ret i16 %3
}

define i32 @_L1X6op_mul2S1X2u8S1X3u32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mul2S1X3u32S1X2u8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_mul2S1X2u8S1X3u64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u64S1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X2u8S1X5usize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5usizeS1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i16 @_L1X6op_mul2S1X3u16S1X3u16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_mul2S1X3u16S1X3u32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mul2S1X3u32S1X3u16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @_L1X6op_mul2S1X3u16S1X3u64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u64S1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u16S1X5usize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5usizeS1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i32 @_L1X6op_mul2S1X3u32S1X3u32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_mul2S1X3u32S1X3u64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u64S1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u32S1X5usize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X5usizeS1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @_L1X6op_mul2S1X3u64S1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X3u64S1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X5usizeS1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mul2S1X5usizeS1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define half @_L1X6op_mul2S1X3f16S1X3f16(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fmul half %0, %1
  ret half %2
}

define float @_L1X6op_mul2S1X3f16S1X3f32(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @_L1X6op_mul2S1X3f32S1X3f16(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @_L1X6op_mul2S1X3f16S1X3f64(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3f16(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define float @_L1X6op_mul2S1X3f32S1X3f32(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fmul float %0, %1
  ret float %2
}

define double @_L1X6op_mul2S1X3f32S1X3f64(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3f32(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define double @_L1X6op_mul2S1X3f64S1X3f64(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fmul double %0, %1
  ret double %2
}

define half @_L1X6op_div2S1X2u8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X2u8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X2u8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X2u8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X2u8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X2u8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3u16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3u16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3u16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3u16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3u16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3u16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3u32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3u32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3u32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3u32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3u32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3u32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3u64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3u64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3u64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3u64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3u64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3u64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X5usizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X5usize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X5usizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X5usize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X5usizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X5usize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X2i8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X2i8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X2i8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X2i8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X2i8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X2i8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3i16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3i16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3i16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3i16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3i16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3i16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3i32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3i32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3i32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3i32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3i32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3i32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X3i64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X3i64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X3i64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3i64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3i64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3i64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @_L1X6op_div2S1X5isizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @_L1X6op_div2S1X3f16S1X5isize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @_L1X6op_div2S1X5isizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X5isize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X5isizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X5isize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define i8 @_L1X6op_div2S1X2i8S1X2i8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_div2S1X2i8S1X3i16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sdiv i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_div2S1X3i16S1X2i8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sdiv i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_div2S1X2i8S1X3i32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_div2S1X3i32S1X2i8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_div2S1X2i8S1X3i64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i64S1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X2i8S1X5isize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5isizeS1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_div2S1X3i16S1X3i16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_div2S1X3i16S1X3i32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_div2S1X3i32S1X3i16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_div2S1X3i16S1X3i64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i64S1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i16S1X5isize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5isizeS1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_div2S1X3i32S1X3i32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_div2S1X3i32S1X3i64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i64S1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i32S1X5isize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5isizeS1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3i64S1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X3i64S1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X5isizeS1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X5isizeS1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i8 @_L1X6op_div2S1X2u8S1X2u8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = udiv i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_div2S1X2u8S1X3u16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i16
  %3 = udiv i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_div2S1X3u16S1X2u8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i16
  %3 = udiv i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_div2S1X2u8S1X3u32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i32
  %3 = udiv i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_div2S1X3u32S1X2u8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i32
  %3 = udiv i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_div2S1X2u8S1X3u64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u64S1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X2u8S1X5usize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5usizeS1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_div2S1X3u16S1X3u16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = udiv i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_div2S1X3u16S1X3u32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i32
  %3 = udiv i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_div2S1X3u32S1X3u16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i32
  %3 = udiv i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_div2S1X3u16S1X3u64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u64S1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u16S1X5usize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5usizeS1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_div2S1X3u32S1X3u32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = udiv i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_div2S1X3u32S1X3u64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u64S1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u32S1X5usize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = udiv i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_div2S1X5usizeS1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = udiv i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_div2S1X3u64S1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X3u64S1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X5usizeS1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_div2S1X5usizeS1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = udiv i64 %0, %1
  ret i64 %2
}

define half @_L1X6op_div2S1X3f16S1X3f16(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fdiv half %0, %1
  ret half %2
}

define float @_L1X6op_div2S1X3f16S1X3f32(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @_L1X6op_div2S1X3f32S1X3f16(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @_L1X6op_div2S1X3f16S1X3f64(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3f16(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define float @_L1X6op_div2S1X3f32S1X3f32(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fdiv float %0, %1
  ret float %2
}

define double @_L1X6op_div2S1X3f32S1X3f64(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3f32(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define double @_L1X6op_div2S1X3f64S1X3f64(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fdiv double %0, %1
  ret double %2
}

define half @_L1X6op_mod2S1X2u8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X2u8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X2u8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X2u8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X2u8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X2u8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i8 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3u16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3u16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3u16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3u16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3u16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3u16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i16 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3u32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3u32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3u32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3u32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3u32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3u32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i32 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3u64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3u64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3u64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3u64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3u64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3u64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X5usizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X5usize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X5usizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X5usize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X5usizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X5usize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = uitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X2i8S1X3f16(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X2i8(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X2i8S1X3f32(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X2i8(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X2i8S1X3f64(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X2i8(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3i16S1X3f16(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3i16(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3i16S1X3f32(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3i16(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3i16S1X3f64(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3i16(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3i32S1X3f16(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3i32(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3i32S1X3f32(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3i32(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3i32S1X3f64(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3i32(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X3i64S1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X3i64(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X3i64S1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3i64(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3i64S1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3i64(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @_L1X6op_mod2S1X5isizeS1X3f16(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @_L1X6op_mod2S1X3f16S1X5isize(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @_L1X6op_mod2S1X5isizeS1X3f32(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X5isize(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X5isizeS1X3f64(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X5isize(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define i8 @_L1X6op_mod2S1X2i8S1X2i8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = srem i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_mod2S1X2i8S1X3i16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = srem i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_mod2S1X3i16S1X2i8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = srem i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_mod2S1X2i8S1X3i32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mod2S1X3i32S1X2i8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_mod2S1X2i8S1X3i64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i64S1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X2i8S1X5isize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5isizeS1X2i8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_mod2S1X3i16S1X3i16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = srem i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_mod2S1X3i16S1X3i32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mod2S1X3i32S1X3i16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_mod2S1X3i16S1X3i64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i64S1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i16S1X5isize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5isizeS1X3i16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_mod2S1X3i32S1X3i32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = srem i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_mod2S1X3i32S1X3i64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i64S1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i32S1X5isize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5isizeS1X3i32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3i64S1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X3i64S1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X5isizeS1X3i64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X5isizeS1X5isize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i8 @_L1X6op_mod2S1X2u8S1X2u8(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = urem i8 %0, %1
  ret i8 %2
}

define i16 @_L1X6op_mod2S1X2u8S1X3u16(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i16
  %3 = urem i16 %2, %1
  ret i16 %3
}

define i16 @_L1X6op_mod2S1X3u16S1X2u8(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i16
  %3 = urem i16 %0, %2
  ret i16 %3
}

define i32 @_L1X6op_mod2S1X2u8S1X3u32(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i32
  %3 = urem i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mod2S1X3u32S1X2u8(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i32
  %3 = urem i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_mod2S1X2u8S1X3u64(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u64S1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X2u8S1X5usize(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5usizeS1X2u8(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = zext i8 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i16 @_L1X6op_mod2S1X3u16S1X3u16(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = urem i16 %0, %1
  ret i16 %2
}

define i32 @_L1X6op_mod2S1X3u16S1X3u32(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i32
  %3 = urem i32 %2, %1
  ret i32 %3
}

define i32 @_L1X6op_mod2S1X3u32S1X3u16(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i32
  %3 = urem i32 %0, %2
  ret i32 %3
}

define i64 @_L1X6op_mod2S1X3u16S1X3u64(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u64S1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u16S1X5usize(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5usizeS1X3u16(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = zext i16 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i32 @_L1X6op_mod2S1X3u32S1X3u32(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = urem i32 %0, %1
  ret i32 %2
}

define i64 @_L1X6op_mod2S1X3u32S1X3u64(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u64S1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u32S1X5usize(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %0 to i64
  %3 = urem i64 %2, %1
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X5usizeS1X3u32(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = zext i32 %1 to i64
  %3 = urem i64 %0, %2
  ret i64 %3
}

define i64 @_L1X6op_mod2S1X3u64S1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X3u64S1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X5usizeS1X3u64(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define i64 @_L1X6op_mod2S1X5usizeS1X5usize(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = urem i64 %0, %1
  ret i64 %2
}

define half @_L1X6op_mod2S1X3f16S1X3f16(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = frem half %0, %1
  ret half %2
}

define float @_L1X6op_mod2S1X3f16S1X3f32(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @_L1X6op_mod2S1X3f32S1X3f16(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @_L1X6op_mod2S1X3f16S1X3f64(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3f16(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define float @_L1X6op_mod2S1X3f32S1X3f32(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = frem float %0, %1
  ret float %2
}

define double @_L1X6op_mod2S1X3f32S1X3f64(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3f32(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define double @_L1X6op_mod2S1X3f64S1X3f64(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = frem double %0, %1
  ret double %2
}

define void @_L4X3sys2my4code4main0() local_unnamed_addr {
.entry:
  call void (ptr, ...) @printf(ptr noundef nonnull @str, i64 8)
  ret void
}

declare void @printf(ptr, ...) local_unnamed_addr

define void @_L4X3sys2my4code4test2G1X7Wrapper1X22G1X7Wrapper1X8PS1X3u32G1X4Pair2X7S1X3i3221G1X7Wrapper1X7S1X3u32(%_LG1X7Wrapper1X22G1X7Wrapper1X8PS1X3u32 %0, %_LG1X4Pair2X7S1X3i3221G1X7Wrapper1X7S1X3u32 %1) local_unnamed_addr {
.entry:
  ret void
}
