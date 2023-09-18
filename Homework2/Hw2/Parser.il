.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method static public void ParseCalcArguments(string[],
                                                  [out] float64&,
                                                  [out] valuetype Hw2.CalculatorOperation&,
                                                  [out] float64&) cil managed
    {
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brtrue AfterBadLengthException

        ldstr "Wrong amount of arguments"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
        
        AfterBadLengthException:
        ldarg.0
        ldc.i4.0
        ldelem.ref
        ldarg.1
        call bool [System.Runtime]System.Double::TryParse(string, [out] float64&)
        brtrue AfterBadValue1Exception
        
        ldstr "Value 1 must be double"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
       
        AfterBadValue1Exception:
        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stobj Hw2.CalculatorOperation
        
        ldarg.0
        ldc.i4.2
        ldelem.ref
        ldarg.3
        call bool [System.Runtime]System.Double::TryParse(string, [out] float64&)
        brtrue AfterBadValue2Exception
        
        ldstr "Value 2 must be double"
        newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
        throw
        
        AfterBadValue2Exception:
        ret
    }

    .method static private bool IsArgLengthSupported(string[]) cil managed
    {
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret 
    }

    .method static private valuetype Hw2.CalculatorOperation ParseOperation(string) cil managed
    {
        ldarg.0
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Plus
        
        ldarg.0
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Minus
        
        ldarg.0
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Multiply
        
        ldarg.0
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue Divide
        
        ldstr "Wrong operation"
        newobj instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
        throw
        
        Plus:
        ldc.i4.0
        ret   
        
        Minus:
        ldc.i4.1
        ret  
        
        Multiply:
        ldc.i4.2
        ret  
        
        Divide:
        ldc.i4.3
        ret  
    }
}


