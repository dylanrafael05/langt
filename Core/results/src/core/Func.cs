namespace Results;

public delegate Result<R> ResultFunc<T1, R>(T1 a);
public delegate Result<R> ResultFunc<T1, T2, R>(T1 a, T2 b);
public delegate Result<R> ResultFunc<T1, T2, T3, R>(T1 a, T2 b, T3 c);
public delegate Result<R> ResultFunc<T1, T2, T3, T4, R>(T1 a, T2 b, T3 c, T4 d);
public delegate Result<R> ResultFunc<T1, T2, T3, T4, T5, R>(T1 a, T2 b, T3 c, T4 d, T5 e);
public delegate Result<R> ResultFunc<T1, T2, T3, T4, T5, T6, R>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);
public delegate Result<R> ResultFunc<T1, T2, T3, T4, T5, T6, T7, R>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g);
public delegate Result<R> ResultFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g, T8 h);


public delegate Result ResultAction<T1>(T1 a);
public delegate Result ResultAction<T1, T2>(T1 a, T2 b);
public delegate Result ResultAction<T1, T2, T3>(T1 a, T2 b, T3 c);
public delegate Result ResultAction<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d);
public delegate Result ResultAction<T1, T2, T3, T4, T5>(T1 a, T2 b, T3 c, T4 d, T5 e);
public delegate Result ResultAction<T1, T2, T3, T4, T5, T6>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);
public delegate Result ResultAction<T1, T2, T3, T4, T5, T6, T7>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g);
public delegate Result ResultAction<T1, T2, T3, T4, T5, T6, T7, T8>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f, T7 g, T8 h);