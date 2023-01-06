; ModuleID = 'HelloWorld.lgt'
source_filename = "HelloWorld.lgt"

%"<langt>::test" = type {}

@str = private unnamed_addr constant [19 x i8] c"I'm ALIIIIIIIVE!!!\00", align 1

define i8 @"<langt>::op_neg(int8)"(i8 %0) local_unnamed_addr {
entry:
  %1 = sub i8 0, %0
  ret i8 %1
}

define i16 @"<langt>::op_neg(int16)"(i16 %0) local_unnamed_addr {
entry:
  %1 = sub i16 0, %0
  ret i16 %1
}

define i32 @"<langt>::op_neg(int32)"(i32 %0) local_unnamed_addr {
entry:
  %1 = sub i32 0, %0
  ret i32 %1
}

define i64 @"<langt>::op_neg(int64)"(i64 %0) local_unnamed_addr {
entry:
  %1 = sub i64 0, %0
  ret i64 %1
}

define half @"<langt>::op_neg(real16)"(half %0) local_unnamed_addr {
entry:
  %1 = fneg half %0
  ret half %1
}

define float @"<langt>::op_neg(real32)"(float %0) local_unnamed_addr {
entry:
  %1 = fneg float %0
  ret float %1
}

define double @"<langt>::op_neg(real64)"(double %0) local_unnamed_addr {
entry:
  %1 = fneg double %0
  ret double %1
}

define half @"<langt>::op_add(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fadd half %int8.to.real16, %1
  ret half %2
}

define half @"<langt>::op_add(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fadd half %int8.to.real16, %0
  ret half %2
}

define half @"<langt>::op_sub(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fsub half %int8.to.real16, %1
  ret half %2
}

define half @"<langt>::op_sub(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fsub half %0, %int8.to.real16
  ret half %2
}

define half @"<langt>::op_mul(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fmul half %int8.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mul(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fmul half %int8.to.real16, %0
  ret half %2
}

define half @"<langt>::op_div(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fdiv half %int8.to.real16, %1
  ret half %2
}

define half @"<langt>::op_div(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fdiv half %0, %int8.to.real16
  ret half %2
}

define half @"<langt>::op_mod(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = frem half %int8.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mod(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = frem half %0, %int8.to.real16
  ret half %2
}

define i1 @"<langt>::op_equal(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp oeq half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp oeq half %int8.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp one half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp one half %int8.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp olt half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp ogt half %int8.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp ole half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp oge half %int8.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp ogt half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp olt half %int8.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,real16)"(i8 %0, half %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %0 to half
  %2 = fcmp oge half %int8.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,int8)"(half %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real16 = sitofp i8 %1 to half
  %2 = fcmp ole half %int8.to.real16, %0
  ret i1 %2
}

define float @"<langt>::op_add(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fadd float %int8.to.real32, %1
  ret float %2
}

define float @"<langt>::op_add(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fadd float %int8.to.real32, %0
  ret float %2
}

define float @"<langt>::op_sub(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fsub float %int8.to.real32, %1
  ret float %2
}

define float @"<langt>::op_sub(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fsub float %0, %int8.to.real32
  ret float %2
}

define float @"<langt>::op_mul(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fmul float %int8.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mul(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fmul float %int8.to.real32, %0
  ret float %2
}

define float @"<langt>::op_div(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fdiv float %int8.to.real32, %1
  ret float %2
}

define float @"<langt>::op_div(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fdiv float %0, %int8.to.real32
  ret float %2
}

define float @"<langt>::op_mod(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = frem float %int8.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mod(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = frem float %0, %int8.to.real32
  ret float %2
}

define i1 @"<langt>::op_equal(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp oeq float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp oeq float %int8.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp one float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp one float %int8.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp olt float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp ogt float %int8.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp ole float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp oge float %int8.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp ogt float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp olt float %int8.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,real32)"(i8 %0, float %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %0 to float
  %2 = fcmp oge float %int8.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,int8)"(float %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real32 = sitofp i8 %1 to float
  %2 = fcmp ole float %int8.to.real32, %0
  ret i1 %2
}

define double @"<langt>::op_add(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fadd double %int8.to.real64, %1
  ret double %2
}

define double @"<langt>::op_add(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fadd double %int8.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fsub double %int8.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fsub double %0, %int8.to.real64
  ret double %2
}

define double @"<langt>::op_mul(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fmul double %int8.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fmul double %int8.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fdiv double %int8.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fdiv double %0, %int8.to.real64
  ret double %2
}

define double @"<langt>::op_mod(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = frem double %int8.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = frem double %0, %int8.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp oeq double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp oeq double %int8.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp one double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp one double %int8.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp olt double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp ogt double %int8.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp ole double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp oge double %int8.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp ogt double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp olt double %int8.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,real64)"(i8 %0, double %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %0 to double
  %2 = fcmp oge double %int8.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,int8)"(double %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.real64 = sitofp i8 %1 to double
  %2 = fcmp ole double %int8.to.real64, %0
  ret i1 %2
}

define half @"<langt>::op_add(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fadd half %int16.to.real16, %1
  ret half %2
}

define half @"<langt>::op_add(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fadd half %int16.to.real16, %0
  ret half %2
}

define half @"<langt>::op_sub(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fsub half %int16.to.real16, %1
  ret half %2
}

define half @"<langt>::op_sub(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fsub half %0, %int16.to.real16
  ret half %2
}

define half @"<langt>::op_mul(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fmul half %int16.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mul(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fmul half %int16.to.real16, %0
  ret half %2
}

define half @"<langt>::op_div(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fdiv half %int16.to.real16, %1
  ret half %2
}

define half @"<langt>::op_div(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fdiv half %0, %int16.to.real16
  ret half %2
}

define half @"<langt>::op_mod(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = frem half %int16.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mod(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = frem half %0, %int16.to.real16
  ret half %2
}

define i1 @"<langt>::op_equal(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp oeq half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp oeq half %int16.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp one half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp one half %int16.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp olt half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp ogt half %int16.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp ole half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp oge half %int16.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp ogt half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp olt half %int16.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,real16)"(i16 %0, half %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %0 to half
  %2 = fcmp oge half %int16.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,int16)"(half %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real16 = sitofp i16 %1 to half
  %2 = fcmp ole half %int16.to.real16, %0
  ret i1 %2
}

define float @"<langt>::op_add(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fadd float %int16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_add(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fadd float %int16.to.real32, %0
  ret float %2
}

define float @"<langt>::op_sub(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fsub float %int16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_sub(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fsub float %0, %int16.to.real32
  ret float %2
}

define float @"<langt>::op_mul(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fmul float %int16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mul(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fmul float %int16.to.real32, %0
  ret float %2
}

define float @"<langt>::op_div(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fdiv float %int16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_div(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fdiv float %0, %int16.to.real32
  ret float %2
}

define float @"<langt>::op_mod(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = frem float %int16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mod(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = frem float %0, %int16.to.real32
  ret float %2
}

define i1 @"<langt>::op_equal(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp oeq float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp oeq float %int16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp one float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp one float %int16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp olt float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp ogt float %int16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp ole float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp oge float %int16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp ogt float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp olt float %int16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,real32)"(i16 %0, float %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %0 to float
  %2 = fcmp oge float %int16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,int16)"(float %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real32 = sitofp i16 %1 to float
  %2 = fcmp ole float %int16.to.real32, %0
  ret i1 %2
}

define double @"<langt>::op_add(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fadd double %int16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_add(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fadd double %int16.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fsub double %int16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fsub double %0, %int16.to.real64
  ret double %2
}

define double @"<langt>::op_mul(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fmul double %int16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fmul double %int16.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fdiv double %int16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fdiv double %0, %int16.to.real64
  ret double %2
}

define double @"<langt>::op_mod(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = frem double %int16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = frem double %0, %int16.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp oeq double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp oeq double %int16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp one double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp one double %int16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp olt double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp ogt double %int16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp ole double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp oge double %int16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp ogt double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp olt double %int16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,real64)"(i16 %0, double %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %0 to double
  %2 = fcmp oge double %int16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,int16)"(double %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.real64 = sitofp i16 %1 to double
  %2 = fcmp ole double %int16.to.real64, %0
  ret i1 %2
}

define half @"<langt>::op_add(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fadd half %int32.to.real16, %1
  ret half %2
}

define half @"<langt>::op_add(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fadd half %int32.to.real16, %0
  ret half %2
}

define half @"<langt>::op_sub(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fsub half %int32.to.real16, %1
  ret half %2
}

define half @"<langt>::op_sub(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fsub half %0, %int32.to.real16
  ret half %2
}

define half @"<langt>::op_mul(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fmul half %int32.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mul(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fmul half %int32.to.real16, %0
  ret half %2
}

define half @"<langt>::op_div(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fdiv half %int32.to.real16, %1
  ret half %2
}

define half @"<langt>::op_div(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fdiv half %0, %int32.to.real16
  ret half %2
}

define half @"<langt>::op_mod(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = frem half %int32.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mod(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = frem half %0, %int32.to.real16
  ret half %2
}

define i1 @"<langt>::op_equal(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp oeq half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp oeq half %int32.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp one half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp one half %int32.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp olt half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp ogt half %int32.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp ole half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp oge half %int32.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp ogt half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp olt half %int32.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,real16)"(i32 %0, half %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %0 to half
  %2 = fcmp oge half %int32.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,int32)"(half %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real16 = sitofp i32 %1 to half
  %2 = fcmp ole half %int32.to.real16, %0
  ret i1 %2
}

define float @"<langt>::op_add(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fadd float %int32.to.real32, %1
  ret float %2
}

define float @"<langt>::op_add(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fadd float %int32.to.real32, %0
  ret float %2
}

define float @"<langt>::op_sub(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fsub float %int32.to.real32, %1
  ret float %2
}

define float @"<langt>::op_sub(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fsub float %0, %int32.to.real32
  ret float %2
}

define float @"<langt>::op_mul(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fmul float %int32.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mul(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fmul float %int32.to.real32, %0
  ret float %2
}

define float @"<langt>::op_div(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fdiv float %int32.to.real32, %1
  ret float %2
}

define float @"<langt>::op_div(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fdiv float %0, %int32.to.real32
  ret float %2
}

define float @"<langt>::op_mod(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = frem float %int32.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mod(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = frem float %0, %int32.to.real32
  ret float %2
}

define i1 @"<langt>::op_equal(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp oeq float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp oeq float %int32.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp one float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp one float %int32.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp olt float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp ogt float %int32.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp ole float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp oge float %int32.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp ogt float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp olt float %int32.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,real32)"(i32 %0, float %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %0 to float
  %2 = fcmp oge float %int32.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,int32)"(float %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real32 = sitofp i32 %1 to float
  %2 = fcmp ole float %int32.to.real32, %0
  ret i1 %2
}

define double @"<langt>::op_add(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fadd double %int32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_add(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fadd double %int32.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fsub double %int32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fsub double %0, %int32.to.real64
  ret double %2
}

define double @"<langt>::op_mul(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fmul double %int32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fmul double %int32.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fdiv double %int32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fdiv double %0, %int32.to.real64
  ret double %2
}

define double @"<langt>::op_mod(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = frem double %int32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = frem double %0, %int32.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp oeq double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp oeq double %int32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp one double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp one double %int32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp olt double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp ogt double %int32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp ole double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp oge double %int32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp ogt double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp olt double %int32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,real64)"(i32 %0, double %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %0 to double
  %2 = fcmp oge double %int32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,int32)"(double %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.real64 = sitofp i32 %1 to double
  %2 = fcmp ole double %int32.to.real64, %0
  ret i1 %2
}

define half @"<langt>::op_add(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fadd half %int64.to.real16, %1
  ret half %2
}

define half @"<langt>::op_add(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fadd half %int64.to.real16, %0
  ret half %2
}

define half @"<langt>::op_sub(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fsub half %int64.to.real16, %1
  ret half %2
}

define half @"<langt>::op_sub(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fsub half %0, %int64.to.real16
  ret half %2
}

define half @"<langt>::op_mul(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fmul half %int64.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mul(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fmul half %int64.to.real16, %0
  ret half %2
}

define half @"<langt>::op_div(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fdiv half %int64.to.real16, %1
  ret half %2
}

define half @"<langt>::op_div(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fdiv half %0, %int64.to.real16
  ret half %2
}

define half @"<langt>::op_mod(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = frem half %int64.to.real16, %1
  ret half %2
}

define half @"<langt>::op_mod(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = frem half %0, %int64.to.real16
  ret half %2
}

define i1 @"<langt>::op_equal(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp oeq half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp oeq half %int64.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp one half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp one half %int64.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp olt half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp ogt half %int64.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp ole half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp oge half %int64.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp ogt half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp olt half %int64.to.real16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,real16)"(i64 %0, half %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %0 to half
  %2 = fcmp oge half %int64.to.real16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,int64)"(half %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real16 = sitofp i64 %1 to half
  %2 = fcmp ole half %int64.to.real16, %0
  ret i1 %2
}

define float @"<langt>::op_add(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fadd float %int64.to.real32, %1
  ret float %2
}

define float @"<langt>::op_add(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fadd float %int64.to.real32, %0
  ret float %2
}

define float @"<langt>::op_sub(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fsub float %int64.to.real32, %1
  ret float %2
}

define float @"<langt>::op_sub(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fsub float %0, %int64.to.real32
  ret float %2
}

define float @"<langt>::op_mul(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fmul float %int64.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mul(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fmul float %int64.to.real32, %0
  ret float %2
}

define float @"<langt>::op_div(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fdiv float %int64.to.real32, %1
  ret float %2
}

define float @"<langt>::op_div(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fdiv float %0, %int64.to.real32
  ret float %2
}

define float @"<langt>::op_mod(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = frem float %int64.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mod(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = frem float %0, %int64.to.real32
  ret float %2
}

define i1 @"<langt>::op_equal(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp oeq float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp oeq float %int64.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp one float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp one float %int64.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp olt float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp ogt float %int64.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp ole float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp oge float %int64.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp ogt float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp olt float %int64.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,real32)"(i64 %0, float %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %0 to float
  %2 = fcmp oge float %int64.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,int64)"(float %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real32 = sitofp i64 %1 to float
  %2 = fcmp ole float %int64.to.real32, %0
  ret i1 %2
}

define double @"<langt>::op_add(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fadd double %int64.to.real64, %1
  ret double %2
}

define double @"<langt>::op_add(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fadd double %int64.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fsub double %int64.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fsub double %0, %int64.to.real64
  ret double %2
}

define double @"<langt>::op_mul(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fmul double %int64.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fmul double %int64.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fdiv double %int64.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fdiv double %0, %int64.to.real64
  ret double %2
}

define double @"<langt>::op_mod(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = frem double %int64.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = frem double %0, %int64.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp oeq double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_equal(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp oeq double %int64.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp one double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp one double %int64.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp olt double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp ogt double %int64.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp ole double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp oge double %int64.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp ogt double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp olt double %int64.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,real64)"(i64 %0, double %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %0 to double
  %2 = fcmp oge double %int64.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,int64)"(double %0, i64 %1) local_unnamed_addr {
entry:
  %int64.to.real64 = sitofp i64 %1 to double
  %2 = fcmp ole double %int64.to.real64, %0
  ret i1 %2
}

define i8 @"<langt>::op_add(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = add i8 %0, %1
  ret i8 %2
}

define i8 @"<langt>::op_sub(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = sub i8 %0, %1
  ret i8 %2
}

define i8 @"<langt>::op_mul(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = mul i8 %0, %1
  ret i8 %2
}

define i8 @"<langt>::op_div(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = sdiv i8 %0, %1
  ret i8 %2
}

define i8 @"<langt>::op_mod(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = srem i8 %0, %1
  ret i8 %2
}

define i1 @"<langt>::op_equal(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp eq i8 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp ne i8 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp slt i8 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp sle i8 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp sgt i8 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,int8)"(i8 %0, i8 %1) local_unnamed_addr {
entry:
  %2 = icmp sge i8 %0, %1
  ret i1 %2
}

define i16 @"<langt>::op_add(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = add i16 %int8.to.int16, %1
  ret i16 %2
}

define i16 @"<langt>::op_sub(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = sub i16 %int8.to.int16, %1
  ret i16 %2
}

define i16 @"<langt>::op_mul(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = mul i16 %int8.to.int16, %1
  ret i16 %2
}

define i16 @"<langt>::op_div(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = sdiv i16 %int8.to.int16, %1
  ret i16 %2
}

define i16 @"<langt>::op_mod(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = srem i16 %int8.to.int16, %1
  ret i16 %2
}

define i1 @"<langt>::op_equal(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp eq i16 %int8.to.int16, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp ne i16 %int8.to.int16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp slt i16 %int8.to.int16, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp sle i16 %int8.to.int16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp sgt i16 %int8.to.int16, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,int16)"(i8 %0, i16 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %0 to i16
  %2 = icmp sge i16 %int8.to.int16, %1
  ret i1 %2
}

define i32 @"<langt>::op_add(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = add i32 %int8.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_sub(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = sub i32 %int8.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_mul(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = mul i32 %int8.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_div(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = sdiv i32 %int8.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_mod(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = srem i32 %int8.to.int32, %1
  ret i32 %2
}

define i1 @"<langt>::op_equal(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp eq i32 %int8.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp ne i32 %int8.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp slt i32 %int8.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp sle i32 %int8.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp sgt i32 %int8.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,int32)"(i8 %0, i32 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %0 to i32
  %2 = icmp sge i32 %int8.to.int32, %1
  ret i1 %2
}

define i64 @"<langt>::op_add(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = add i64 %int8.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_sub(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = sub i64 %int8.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mul(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = mul i64 %int8.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_div(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = sdiv i64 %int8.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mod(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = srem i64 %int8.to.int64, %1
  ret i64 %2
}

define i1 @"<langt>::op_equal(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp eq i64 %int8.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp ne i64 %int8.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp slt i64 %int8.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp sle i64 %int8.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp sgt i64 %int8.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int8,int64)"(i8 %0, i64 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %0 to i64
  %2 = icmp sge i64 %int8.to.int64, %1
  ret i1 %2
}

define i16 @"<langt>::op_add(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = add i16 %int8.to.int16, %0
  ret i16 %2
}

define i16 @"<langt>::op_sub(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = sub i16 %0, %int8.to.int16
  ret i16 %2
}

define i16 @"<langt>::op_mul(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = mul i16 %int8.to.int16, %0
  ret i16 %2
}

define i16 @"<langt>::op_div(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = sdiv i16 %0, %int8.to.int16
  ret i16 %2
}

define i16 @"<langt>::op_mod(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = srem i16 %0, %int8.to.int16
  ret i16 %2
}

define i1 @"<langt>::op_equal(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp eq i16 %int8.to.int16, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp ne i16 %int8.to.int16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp sgt i16 %int8.to.int16, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp sge i16 %int8.to.int16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp slt i16 %int8.to.int16, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,int8)"(i16 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int16 = sext i8 %1 to i16
  %2 = icmp sle i16 %int8.to.int16, %0
  ret i1 %2
}

define i16 @"<langt>::op_add(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = add i16 %0, %1
  ret i16 %2
}

define i16 @"<langt>::op_sub(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = sub i16 %0, %1
  ret i16 %2
}

define i16 @"<langt>::op_mul(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = mul i16 %0, %1
  ret i16 %2
}

define i16 @"<langt>::op_div(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = sdiv i16 %0, %1
  ret i16 %2
}

define i16 @"<langt>::op_mod(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = srem i16 %0, %1
  ret i16 %2
}

define i1 @"<langt>::op_equal(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp eq i16 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp ne i16 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp slt i16 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp sle i16 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp sgt i16 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,int16)"(i16 %0, i16 %1) local_unnamed_addr {
entry:
  %2 = icmp sge i16 %0, %1
  ret i1 %2
}

define i32 @"<langt>::op_add(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = add i32 %int16.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_sub(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = sub i32 %int16.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_mul(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = mul i32 %int16.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_div(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = sdiv i32 %int16.to.int32, %1
  ret i32 %2
}

define i32 @"<langt>::op_mod(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = srem i32 %int16.to.int32, %1
  ret i32 %2
}

define i1 @"<langt>::op_equal(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp eq i32 %int16.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp ne i32 %int16.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp slt i32 %int16.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp sle i32 %int16.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp sgt i32 %int16.to.int32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,int32)"(i16 %0, i32 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %0 to i32
  %2 = icmp sge i32 %int16.to.int32, %1
  ret i1 %2
}

define i64 @"<langt>::op_add(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = add i64 %int16.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_sub(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = sub i64 %int16.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mul(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = mul i64 %int16.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_div(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = sdiv i64 %int16.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mod(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = srem i64 %int16.to.int64, %1
  ret i64 %2
}

define i1 @"<langt>::op_equal(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp eq i64 %int16.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp ne i64 %int16.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp slt i64 %int16.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp sle i64 %int16.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp sgt i64 %int16.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int16,int64)"(i16 %0, i64 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %0 to i64
  %2 = icmp sge i64 %int16.to.int64, %1
  ret i1 %2
}

define i32 @"<langt>::op_add(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = add i32 %int8.to.int32, %0
  ret i32 %2
}

define i32 @"<langt>::op_sub(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = sub i32 %0, %int8.to.int32
  ret i32 %2
}

define i32 @"<langt>::op_mul(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = mul i32 %int8.to.int32, %0
  ret i32 %2
}

define i32 @"<langt>::op_div(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = sdiv i32 %0, %int8.to.int32
  ret i32 %2
}

define i32 @"<langt>::op_mod(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = srem i32 %0, %int8.to.int32
  ret i32 %2
}

define i1 @"<langt>::op_equal(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp eq i32 %int8.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp ne i32 %int8.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp sgt i32 %int8.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp sge i32 %int8.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp slt i32 %int8.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,int8)"(i32 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int32 = sext i8 %1 to i32
  %2 = icmp sle i32 %int8.to.int32, %0
  ret i1 %2
}

define i32 @"<langt>::op_add(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = add i32 %int16.to.int32, %0
  ret i32 %2
}

define i32 @"<langt>::op_sub(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = sub i32 %0, %int16.to.int32
  ret i32 %2
}

define i32 @"<langt>::op_mul(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = mul i32 %int16.to.int32, %0
  ret i32 %2
}

define i32 @"<langt>::op_div(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = sdiv i32 %0, %int16.to.int32
  ret i32 %2
}

define i32 @"<langt>::op_mod(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = srem i32 %0, %int16.to.int32
  ret i32 %2
}

define i1 @"<langt>::op_equal(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp eq i32 %int16.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp ne i32 %int16.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp sgt i32 %int16.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp sge i32 %int16.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp slt i32 %int16.to.int32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,int16)"(i32 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int32 = sext i16 %1 to i32
  %2 = icmp sle i32 %int16.to.int32, %0
  ret i1 %2
}

define i32 @"<langt>::op_add(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = add i32 %0, %1
  ret i32 %2
}

define i32 @"<langt>::op_sub(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = sub i32 %0, %1
  ret i32 %2
}

define i32 @"<langt>::op_mul(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = mul i32 %0, %1
  ret i32 %2
}

define i32 @"<langt>::op_div(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = sdiv i32 %0, %1
  ret i32 %2
}

define i32 @"<langt>::op_mod(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = srem i32 %0, %1
  ret i32 %2
}

define i1 @"<langt>::op_equal(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp eq i32 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp ne i32 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp slt i32 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp sle i32 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp sgt i32 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,int32)"(i32 %0, i32 %1) local_unnamed_addr {
entry:
  %2 = icmp sge i32 %0, %1
  ret i1 %2
}

define i64 @"<langt>::op_add(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = add i64 %int32.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_sub(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = sub i64 %int32.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mul(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = mul i64 %int32.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_div(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = sdiv i64 %int32.to.int64, %1
  ret i64 %2
}

define i64 @"<langt>::op_mod(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = srem i64 %int32.to.int64, %1
  ret i64 %2
}

define i1 @"<langt>::op_equal(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp eq i64 %int32.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp ne i64 %int32.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp slt i64 %int32.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp sle i64 %int32.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp sgt i64 %int32.to.int64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int32,int64)"(i32 %0, i64 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %0 to i64
  %2 = icmp sge i64 %int32.to.int64, %1
  ret i1 %2
}

define i64 @"<langt>::op_add(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = add i64 %int8.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_sub(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = sub i64 %0, %int8.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mul(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = mul i64 %int8.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_div(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = sdiv i64 %0, %int8.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mod(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = srem i64 %0, %int8.to.int64
  ret i64 %2
}

define i1 @"<langt>::op_equal(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp eq i64 %int8.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp ne i64 %int8.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp sgt i64 %int8.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp sge i64 %int8.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp slt i64 %int8.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,int8)"(i64 %0, i8 %1) local_unnamed_addr {
entry:
  %int8.to.int64 = sext i8 %1 to i64
  %2 = icmp sle i64 %int8.to.int64, %0
  ret i1 %2
}

define i64 @"<langt>::op_add(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = add i64 %int16.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_sub(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = sub i64 %0, %int16.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mul(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = mul i64 %int16.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_div(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = sdiv i64 %0, %int16.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mod(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = srem i64 %0, %int16.to.int64
  ret i64 %2
}

define i1 @"<langt>::op_equal(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp eq i64 %int16.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp ne i64 %int16.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp sgt i64 %int16.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp sge i64 %int16.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp slt i64 %int16.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,int16)"(i64 %0, i16 %1) local_unnamed_addr {
entry:
  %int16.to.int64 = sext i16 %1 to i64
  %2 = icmp sle i64 %int16.to.int64, %0
  ret i1 %2
}

define i64 @"<langt>::op_add(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = add i64 %int32.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_sub(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = sub i64 %0, %int32.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mul(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = mul i64 %int32.to.int64, %0
  ret i64 %2
}

define i64 @"<langt>::op_div(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = sdiv i64 %0, %int32.to.int64
  ret i64 %2
}

define i64 @"<langt>::op_mod(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = srem i64 %0, %int32.to.int64
  ret i64 %2
}

define i1 @"<langt>::op_equal(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp eq i64 %int32.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp ne i64 %int32.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp sgt i64 %int32.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp sge i64 %int32.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp slt i64 %int32.to.int64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,int32)"(i64 %0, i32 %1) local_unnamed_addr {
entry:
  %int32.to.int64 = sext i32 %1 to i64
  %2 = icmp sle i64 %int32.to.int64, %0
  ret i1 %2
}

define i64 @"<langt>::op_add(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = add i64 %0, %1
  ret i64 %2
}

define i64 @"<langt>::op_sub(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = sub i64 %0, %1
  ret i64 %2
}

define i64 @"<langt>::op_mul(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = mul i64 %0, %1
  ret i64 %2
}

define i64 @"<langt>::op_div(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = sdiv i64 %0, %1
  ret i64 %2
}

define i64 @"<langt>::op_mod(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = srem i64 %0, %1
  ret i64 %2
}

define i1 @"<langt>::op_equal(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp eq i64 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp ne i64 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp slt i64 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp sle i64 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp sgt i64 %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(int64,int64)"(i64 %0, i64 %1) local_unnamed_addr {
entry:
  %2 = icmp sge i64 %0, %1
  ret i1 %2
}

define half @"<langt>::op_add(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fadd half %0, %1
  ret half %2
}

define half @"<langt>::op_sub(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fsub half %0, %1
  ret half %2
}

define half @"<langt>::op_mul(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fmul half %0, %1
  ret half %2
}

define half @"<langt>::op_div(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fdiv half %0, %1
  ret half %2
}

define half @"<langt>::op_mod(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = frem half %0, %1
  ret half %2
}

define i1 @"<langt>::op_equal(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp oeq half %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp one half %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp olt half %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp ole half %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp ogt half %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,real16)"(half %0, half %1) local_unnamed_addr {
entry:
  %2 = fcmp oge half %0, %1
  ret i1 %2
}

define float @"<langt>::op_add(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fadd float %real16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_sub(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fsub float %real16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mul(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fmul float %real16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_div(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fdiv float %real16.to.real32, %1
  ret float %2
}

define float @"<langt>::op_mod(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = frem float %real16.to.real32, %1
  ret float %2
}

define i1 @"<langt>::op_equal(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp oeq float %real16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp one float %real16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp olt float %real16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp ole float %real16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp ogt float %real16.to.real32, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,real32)"(half %0, float %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %0 to float
  %2 = fcmp oge float %real16.to.real32, %1
  ret i1 %2
}

define double @"<langt>::op_add(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fadd double %real16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fsub double %real16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fmul double %real16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fdiv double %real16.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = frem double %real16.to.real64, %1
  ret double %2
}

define i1 @"<langt>::op_equal(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp oeq double %real16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp one double %real16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp olt double %real16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp ole double %real16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp ogt double %real16.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real16,real64)"(half %0, double %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %0 to double
  %2 = fcmp oge double %real16.to.real64, %1
  ret i1 %2
}

define float @"<langt>::op_add(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fadd float %real16.to.real32, %0
  ret float %2
}

define float @"<langt>::op_sub(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fsub float %0, %real16.to.real32
  ret float %2
}

define float @"<langt>::op_mul(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fmul float %real16.to.real32, %0
  ret float %2
}

define float @"<langt>::op_div(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fdiv float %0, %real16.to.real32
  ret float %2
}

define float @"<langt>::op_mod(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = frem float %0, %real16.to.real32
  ret float %2
}

define i1 @"<langt>::op_equal(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp oeq float %real16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp one float %real16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp ogt float %real16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp oge float %real16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp olt float %real16.to.real32, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,real16)"(float %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real32 = fpext half %1 to float
  %2 = fcmp ole float %real16.to.real32, %0
  ret i1 %2
}

define float @"<langt>::op_add(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fadd float %0, %1
  ret float %2
}

define float @"<langt>::op_sub(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fsub float %0, %1
  ret float %2
}

define float @"<langt>::op_mul(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fmul float %0, %1
  ret float %2
}

define float @"<langt>::op_div(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fdiv float %0, %1
  ret float %2
}

define float @"<langt>::op_mod(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = frem float %0, %1
  ret float %2
}

define i1 @"<langt>::op_equal(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp oeq float %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp one float %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp olt float %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp ole float %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp ogt float %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,real32)"(float %0, float %1) local_unnamed_addr {
entry:
  %2 = fcmp oge float %0, %1
  ret i1 %2
}

define double @"<langt>::op_add(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fadd double %real32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_sub(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fsub double %real32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mul(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fmul double %real32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_div(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fdiv double %real32.to.real64, %1
  ret double %2
}

define double @"<langt>::op_mod(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = frem double %real32.to.real64, %1
  ret double %2
}

define i1 @"<langt>::op_equal(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp oeq double %real32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp one double %real32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp olt double %real32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp ole double %real32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp ogt double %real32.to.real64, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real32,real64)"(float %0, double %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %0 to double
  %2 = fcmp oge double %real32.to.real64, %1
  ret i1 %2
}

define double @"<langt>::op_add(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fadd double %real16.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fsub double %0, %real16.to.real64
  ret double %2
}

define double @"<langt>::op_mul(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fmul double %real16.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fdiv double %0, %real16.to.real64
  ret double %2
}

define double @"<langt>::op_mod(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = frem double %0, %real16.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp oeq double %real16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp one double %real16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp ogt double %real16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp oge double %real16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp olt double %real16.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,real16)"(double %0, half %1) local_unnamed_addr {
entry:
  %real16.to.real64 = fpext half %1 to double
  %2 = fcmp ole double %real16.to.real64, %0
  ret i1 %2
}

define double @"<langt>::op_add(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fadd double %real32.to.real64, %0
  ret double %2
}

define double @"<langt>::op_sub(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fsub double %0, %real32.to.real64
  ret double %2
}

define double @"<langt>::op_mul(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fmul double %real32.to.real64, %0
  ret double %2
}

define double @"<langt>::op_div(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fdiv double %0, %real32.to.real64
  ret double %2
}

define double @"<langt>::op_mod(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = frem double %0, %real32.to.real64
  ret double %2
}

define i1 @"<langt>::op_equal(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp oeq double %real32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp one double %real32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp ogt double %real32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp oge double %real32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp olt double %real32.to.real64, %0
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,real32)"(double %0, float %1) local_unnamed_addr {
entry:
  %real32.to.real64 = fpext float %1 to double
  %2 = fcmp ole double %real32.to.real64, %0
  ret i1 %2
}

define double @"<langt>::op_add(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fadd double %0, %1
  ret double %2
}

define double @"<langt>::op_sub(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fsub double %0, %1
  ret double %2
}

define double @"<langt>::op_mul(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fmul double %0, %1
  ret double %2
}

define double @"<langt>::op_div(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fdiv double %0, %1
  ret double %2
}

define double @"<langt>::op_mod(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = frem double %0, %1
  ret double %2
}

define i1 @"<langt>::op_equal(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp oeq double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not_equal(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp one double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp olt double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_less_equal(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp ole double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp ogt double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_greater_equal(real64,real64)"(double %0, double %1) local_unnamed_addr {
entry:
  %2 = fcmp oge double %0, %1
  ret i1 %2
}

define i1 @"<langt>::op_not(bool)"(i1 %0) local_unnamed_addr {
entry:
  %1 = xor i1 %0, true
  ret i1 %1
}

declare void @printf(ptr, ...) local_unnamed_addr

define void @"<langt>::main()"() local_unnamed_addr {
entry:
  call void (ptr, ...) @printf(ptr noundef nonnull @str)
  ret void
}

define %"<langt>::test" @"<langt>::make_test()"() local_unnamed_addr {
entry:
  ret %"<langt>::test" undef
}
