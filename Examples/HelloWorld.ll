; ModuleID = 'HelloWorld.lgt'
source_filename = "HelloWorld.lgt"

@str = private unnamed_addr constant [12 x i8] c"HALLO WARLD\00", align 1

define i1 @"<lgt>operator not(bool)"(i1 %0) local_unnamed_addr {
.entry:
  %1 = xor i1 %0, true
  ret i1 %1
}

define i8 @"<lgt>operator unary -(int8)"(i8 %0) local_unnamed_addr {
.entry:
  %1 = sub i8 0, %0
  ret i8 %1
}

define i16 @"<lgt>operator unary -(int16)"(i16 %0) local_unnamed_addr {
.entry:
  %1 = sub i16 0, %0
  ret i16 %1
}

define i32 @"<lgt>operator unary -(int32)"(i32 %0) local_unnamed_addr {
.entry:
  %1 = sub i32 0, %0
  ret i32 %1
}

define i64 @"<lgt>operator unary -(int64)"(i64 %0) local_unnamed_addr {
.entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define half @"<lgt>operator unary -(real16)"(half %0) local_unnamed_addr {
.entry:
  %1 = fneg half %0
  ret half %1
}

define float @"<lgt>operator unary -(real32)"(float %0) local_unnamed_addr {
.entry:
  %1 = fneg float %0
  ret float %1
}

define double @"<lgt>operator unary -(real64)"(double %0) local_unnamed_addr {
.entry:
  %1 = fneg double %0
  ret double %1
}

define half @"<lgt>operator +(int8, real16)"(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(real16, int8)"(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @"<lgt>operator +(int8, real32)"(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(real32, int8)"(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @"<lgt>operator +(int8, real64)"(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, int8)"(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @"<lgt>operator +(int16, real16)"(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(real16, int16)"(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @"<lgt>operator +(int16, real32)"(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(real32, int16)"(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @"<lgt>operator +(int16, real64)"(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, int16)"(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @"<lgt>operator +(int32, real16)"(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(real16, int32)"(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @"<lgt>operator +(int32, real32)"(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(real32, int32)"(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @"<lgt>operator +(int32, real64)"(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, int32)"(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define half @"<lgt>operator +(int64, real16)"(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fadd half %2, %1
  ret half %3
}

define half @"<lgt>operator +(real16, int64)"(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fadd half %2, %0
  ret half %3
}

define float @"<lgt>operator +(int64, real32)"(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(real32, int64)"(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @"<lgt>operator +(int64, real64)"(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, int64)"(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define i8 @"<lgt>operator +(int8, int8)"(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator +(int8, int16)"(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = add i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator +(int16, int8)"(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = add i16 %2, %0
  ret i16 %3
}

define i32 @"<lgt>operator +(int8, int32)"(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(int32, int8)"(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @"<lgt>operator +(int8, int64)"(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(int64, int8)"(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i16 @"<lgt>operator +(int16, int16)"(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator +(int16, int32)"(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = add i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator +(int32, int16)"(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = add i32 %2, %0
  ret i32 %3
}

define i64 @"<lgt>operator +(int16, int64)"(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(int64, int16)"(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i32 @"<lgt>operator +(int32, int32)"(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator +(int32, int64)"(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = add i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator +(int64, int32)"(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = add i64 %2, %0
  ret i64 %3
}

define i64 @"<lgt>operator +(int64, int64)"(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator +(real16, real16)"(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fadd half %0, %1
  ret half %2
}

define float @"<lgt>operator +(real16, real32)"(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fadd float %2, %1
  ret float %3
}

define float @"<lgt>operator +(real32, real16)"(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fadd float %2, %0
  ret float %3
}

define double @"<lgt>operator +(real16, real64)"(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, real16)"(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define float @"<lgt>operator +(real32, real32)"(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fadd float %0, %1
  ret float %2
}

define double @"<lgt>operator +(real32, real64)"(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fadd double %2, %1
  ret double %3
}

define double @"<lgt>operator +(real64, real32)"(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fadd double %2, %0
  ret double %3
}

define double @"<lgt>operator +(real64, real64)"(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fadd double %0, %1
  ret double %2
}

define half @"<lgt>operator -(int8, real16)"(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(real16, int8)"(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(int8, real32)"(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(real32, int8)"(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(int8, real64)"(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, int8)"(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(int16, real16)"(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(real16, int16)"(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(int16, real32)"(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(real32, int16)"(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(int16, real64)"(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, int16)"(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(int32, real16)"(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(real16, int32)"(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(int32, real32)"(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(real32, int32)"(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(int32, real64)"(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, int32)"(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define half @"<lgt>operator -(int64, real16)"(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fsub half %2, %1
  ret half %3
}

define half @"<lgt>operator -(real16, int64)"(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fsub half %0, %2
  ret half %3
}

define float @"<lgt>operator -(int64, real32)"(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(real32, int64)"(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(int64, real64)"(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, int64)"(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define i8 @"<lgt>operator -(int8, int8)"(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator -(int8, int16)"(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sub i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator -(int16, int8)"(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sub i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator -(int8, int32)"(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(int32, int8)"(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(int8, int64)"(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(int64, int8)"(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator -(int16, int16)"(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator -(int16, int32)"(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sub i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator -(int32, int16)"(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sub i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator -(int16, int64)"(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(int64, int16)"(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator -(int32, int32)"(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator -(int32, int64)"(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sub i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator -(int64, int32)"(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sub i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator -(int64, int64)"(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator -(real16, real16)"(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fsub half %0, %1
  ret half %2
}

define float @"<lgt>operator -(real16, real32)"(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fsub float %2, %1
  ret float %3
}

define float @"<lgt>operator -(real32, real16)"(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fsub float %0, %2
  ret float %3
}

define double @"<lgt>operator -(real16, real64)"(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, real16)"(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define float @"<lgt>operator -(real32, real32)"(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fsub float %0, %1
  ret float %2
}

define double @"<lgt>operator -(real32, real64)"(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fsub double %2, %1
  ret double %3
}

define double @"<lgt>operator -(real64, real32)"(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fsub double %0, %2
  ret double %3
}

define double @"<lgt>operator -(real64, real64)"(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fsub double %0, %1
  ret double %2
}

define half @"<lgt>operator *(int8, real16)"(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(real16, int8)"(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @"<lgt>operator *(int8, real32)"(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(real32, int8)"(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @"<lgt>operator *(int8, real64)"(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, int8)"(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @"<lgt>operator *(int16, real16)"(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(real16, int16)"(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @"<lgt>operator *(int16, real32)"(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(real32, int16)"(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @"<lgt>operator *(int16, real64)"(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, int16)"(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @"<lgt>operator *(int32, real16)"(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(real16, int32)"(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @"<lgt>operator *(int32, real32)"(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(real32, int32)"(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @"<lgt>operator *(int32, real64)"(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, int32)"(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define half @"<lgt>operator *(int64, real16)"(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fmul half %2, %1
  ret half %3
}

define half @"<lgt>operator *(real16, int64)"(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fmul half %2, %0
  ret half %3
}

define float @"<lgt>operator *(int64, real32)"(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(real32, int64)"(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @"<lgt>operator *(int64, real64)"(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, int64)"(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define i8 @"<lgt>operator *(int8, int8)"(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator *(int8, int16)"(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = mul i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator *(int16, int8)"(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = mul i16 %2, %0
  ret i16 %3
}

define i32 @"<lgt>operator *(int8, int32)"(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(int32, int8)"(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @"<lgt>operator *(int8, int64)"(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(int64, int8)"(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i16 @"<lgt>operator *(int16, int16)"(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator *(int16, int32)"(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = mul i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator *(int32, int16)"(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = mul i32 %2, %0
  ret i32 %3
}

define i64 @"<lgt>operator *(int16, int64)"(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(int64, int16)"(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i32 @"<lgt>operator *(int32, int32)"(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator *(int32, int64)"(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = mul i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator *(int64, int32)"(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = mul i64 %2, %0
  ret i64 %3
}

define i64 @"<lgt>operator *(int64, int64)"(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator *(real16, real16)"(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fmul half %0, %1
  ret half %2
}

define float @"<lgt>operator *(real16, real32)"(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fmul float %2, %1
  ret float %3
}

define float @"<lgt>operator *(real32, real16)"(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fmul float %2, %0
  ret float %3
}

define double @"<lgt>operator *(real16, real64)"(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, real16)"(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define float @"<lgt>operator *(real32, real32)"(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fmul float %0, %1
  ret float %2
}

define double @"<lgt>operator *(real32, real64)"(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fmul double %2, %1
  ret double %3
}

define double @"<lgt>operator *(real64, real32)"(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fmul double %2, %0
  ret double %3
}

define double @"<lgt>operator *(real64, real64)"(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fmul double %0, %1
  ret double %2
}

define half @"<lgt>operator /(int8, real16)"(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(real16, int8)"(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(int8, real32)"(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(real32, int8)"(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(int8, real64)"(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, int8)"(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(int16, real16)"(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(real16, int16)"(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(int16, real32)"(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(real32, int16)"(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(int16, real64)"(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, int16)"(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(int32, real16)"(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(real16, int32)"(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(int32, real32)"(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(real32, int32)"(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(int32, real64)"(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, int32)"(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define half @"<lgt>operator /(int64, real16)"(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = fdiv half %2, %1
  ret half %3
}

define half @"<lgt>operator /(real16, int64)"(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = fdiv half %0, %2
  ret half %3
}

define float @"<lgt>operator /(int64, real32)"(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(real32, int64)"(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(int64, real64)"(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, int64)"(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define i8 @"<lgt>operator /(int8, int8)"(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator /(int8, int16)"(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = sdiv i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator /(int16, int8)"(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = sdiv i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator /(int8, int32)"(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(int32, int8)"(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(int8, int64)"(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(int64, int8)"(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator /(int16, int16)"(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator /(int16, int32)"(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = sdiv i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator /(int32, int16)"(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = sdiv i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator /(int16, int64)"(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(int64, int16)"(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator /(int32, int32)"(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator /(int32, int64)"(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = sdiv i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator /(int64, int32)"(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = sdiv i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator /(int64, int64)"(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator /(real16, real16)"(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = fdiv half %0, %1
  ret half %2
}

define float @"<lgt>operator /(real16, real32)"(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = fdiv float %2, %1
  ret float %3
}

define float @"<lgt>operator /(real32, real16)"(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = fdiv float %0, %2
  ret float %3
}

define double @"<lgt>operator /(real16, real64)"(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, real16)"(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define float @"<lgt>operator /(real32, real32)"(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = fdiv float %0, %1
  ret float %2
}

define double @"<lgt>operator /(real32, real64)"(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = fdiv double %2, %1
  ret double %3
}

define double @"<lgt>operator /(real64, real32)"(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = fdiv double %0, %2
  ret double %3
}

define double @"<lgt>operator /(real64, real64)"(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = fdiv double %0, %1
  ret double %2
}

define half @"<lgt>operator %(int8, real16)"(i8 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(real16, int8)"(half %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(int8, real32)"(i8 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(real32, int8)"(float %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(int8, real64)"(i8 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, int8)"(double %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i8 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(int16, real16)"(i16 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(real16, int16)"(half %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(int16, real32)"(i16 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(real32, int16)"(float %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(int16, real64)"(i16 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, int16)"(double %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i16 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(int32, real16)"(i32 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(real16, int32)"(half %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(int32, real32)"(i32 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(real32, int32)"(float %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(int32, real64)"(i32 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, int32)"(double %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i32 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define half @"<lgt>operator %(int64, real16)"(i64 %0, half %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to half
  %3 = frem half %2, %1
  ret half %3
}

define half @"<lgt>operator %(real16, int64)"(half %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to half
  %3 = frem half %0, %2
  ret half %3
}

define float @"<lgt>operator %(int64, real32)"(i64 %0, float %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(real32, int64)"(float %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(int64, real64)"(i64 %0, double %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, int64)"(double %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sitofp i64 %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define i8 @"<lgt>operator %(int8, int8)"(i8 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = srem i8 %0, %1
  ret i8 %2
}

define i16 @"<lgt>operator %(int8, int16)"(i8 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i16
  %3 = srem i16 %2, %1
  ret i16 %3
}

define i16 @"<lgt>operator %(int16, int8)"(i16 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i16
  %3 = srem i16 %0, %2
  ret i16 %3
}

define i32 @"<lgt>operator %(int8, int32)"(i8 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(int32, int8)"(i32 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(int8, int64)"(i8 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(int64, int8)"(i64 %0, i8 %1) local_unnamed_addr {
.entry:
  %2 = sext i8 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i16 @"<lgt>operator %(int16, int16)"(i16 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = srem i16 %0, %1
  ret i16 %2
}

define i32 @"<lgt>operator %(int16, int32)"(i16 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i32
  %3 = srem i32 %2, %1
  ret i32 %3
}

define i32 @"<lgt>operator %(int32, int16)"(i32 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i32
  %3 = srem i32 %0, %2
  ret i32 %3
}

define i64 @"<lgt>operator %(int16, int64)"(i16 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(int64, int16)"(i64 %0, i16 %1) local_unnamed_addr {
.entry:
  %2 = sext i16 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i32 @"<lgt>operator %(int32, int32)"(i32 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = srem i32 %0, %1
  ret i32 %2
}

define i64 @"<lgt>operator %(int32, int64)"(i32 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %0 to i64
  %3 = srem i64 %2, %1
  ret i64 %3
}

define i64 @"<lgt>operator %(int64, int32)"(i64 %0, i32 %1) local_unnamed_addr {
.entry:
  %2 = sext i32 %1 to i64
  %3 = srem i64 %0, %2
  ret i64 %3
}

define i64 @"<lgt>operator %(int64, int64)"(i64 %0, i64 %1) local_unnamed_addr {
.entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define half @"<lgt>operator %(real16, real16)"(half %0, half %1) local_unnamed_addr {
.entry:
  %2 = frem half %0, %1
  ret half %2
}

define float @"<lgt>operator %(real16, real32)"(half %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to float
  %3 = frem float %2, %1
  ret float %3
}

define float @"<lgt>operator %(real32, real16)"(float %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to float
  %3 = frem float %0, %2
  ret float %3
}

define double @"<lgt>operator %(real16, real64)"(half %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext half %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, real16)"(double %0, half %1) local_unnamed_addr {
.entry:
  %2 = fpext half %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define float @"<lgt>operator %(real32, real32)"(float %0, float %1) local_unnamed_addr {
.entry:
  %2 = frem float %0, %1
  ret float %2
}

define double @"<lgt>operator %(real32, real64)"(float %0, double %1) local_unnamed_addr {
.entry:
  %2 = fpext float %0 to double
  %3 = frem double %2, %1
  ret double %3
}

define double @"<lgt>operator %(real64, real32)"(double %0, float %1) local_unnamed_addr {
.entry:
  %2 = fpext float %1 to double
  %3 = frem double %0, %2
  ret double %3
}

define double @"<lgt>operator %(real64, real64)"(double %0, double %1) local_unnamed_addr {
.entry:
  %2 = frem double %0, %1
  ret double %2
}

define void @"<lgt>main()"() local_unnamed_addr {
.entry:
  call void (ptr, ...) @printf(ptr noundef nonnull @str)
  ret void
}

declare void @printf(ptr, ...) local_unnamed_addr
