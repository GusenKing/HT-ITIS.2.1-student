.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method static public float64 Calculate(float64, valuetype Hw2.CalculatorOperation, float64) cil managed
    {
        ldarg.1
        ldc.i4.0
        beq Plus
        
        ldarg.1
        ldc.i4.1
        beq Minus
        
        ldarg.1
        ldc.i4.2
        beq Multiply
        
        ldarg.1
        ldc.i4.3
        beq Divide
        
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw
        
        Plus:
        ldarg.0
        ldarg.2
        add
        ret
        
        Minus:
        ldarg.0
        ldarg.2
        sub
        ret
        
        Multiply:
        ldarg.0
        ldarg.2
        mul
        ret
        
        Divide:
        ldarg.0
        ldarg.2
        div
        ret
    }     
}



