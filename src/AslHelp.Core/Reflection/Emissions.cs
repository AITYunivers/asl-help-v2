using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using LiveSplit.ASL;

namespace AslHelp.Core.Reflection;

internal static class Emissions
{
    private static Func<ASLScript, Process> _getScriptGame;
    public static Func<ASLScript, Process> GetScriptGame
    {
        get
        {
            if (_getScriptGame is not null)
            {
                return _getScriptGame;
            }

            DynamicMethod dm = new(nameof(GetScriptGame), typeof(Process), new[] { typeof(ASLScript) }, true);

            FieldInfo fiGame = typeof(ASLScript).GetField("_game", BindingFlags.Instance | BindingFlags.NonPublic);

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fiGame);
            il.Emit(OpCodes.Ret);

            return _getScriptGame = (Func<ASLScript, Process>)dm.CreateDelegate(typeof(Func<ASLScript, Process>));
        }
    }

    private static Action<ASLScript, Process> _setScriptGame;
    public static Action<ASLScript, Process> SetScriptGame
    {
        get
        {
            if (_setScriptGame is not null)
            {
                return _setScriptGame;
            }

            DynamicMethod dm = new(nameof(SetScriptGame), null, new[] { typeof(ASLScript), typeof(Process) }, true);

            FieldInfo fiGame = typeof(ASLScript).GetField("_game", BindingFlags.Instance | BindingFlags.NonPublic);

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fiGame);
            il.Emit(OpCodes.Ret);

            return _setScriptGame = (Action<ASLScript, Process>)dm.CreateDelegate(typeof(Action<ASLScript, Process>));
        }
    }
}

internal static class Emissions<T>
{
    private static Func<List<T>, (T[], int)> _getBackingArray;
    public static Func<List<T>, (T[], int)> GetBackingArray
    {
        get
        {
            if (_getBackingArray is not null)
            {
                return _getBackingArray;
            }

            DynamicMethod dm = new(nameof(GetBackingArray), typeof((T[], int)), new[] { typeof(List<T>) }, true);

            FieldInfo fiItems = typeof(List<T>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo fiSize = typeof(List<T>).GetField("_size", BindingFlags.Instance | BindingFlags.NonPublic);

            ConstructorInfo ctor = typeof((T[], int)).GetConstructor(new[] { typeof(T[]), typeof(int) });

            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fiItems);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fiSize);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            return _getBackingArray = (Func<List<T>, (T[], int)>)dm.CreateDelegate(typeof(Func<List<T>, (T[], int)>));
        }
    }
}
