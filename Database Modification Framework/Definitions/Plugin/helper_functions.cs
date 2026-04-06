using BepInEx;
using Database_Modification_Framework.Database;
using System;
using System.Collections.Generic;

namespace Database_Modification_Framework.Definitions
{
    // Implements a child Unity Plugin class allowing users to use  classses
    // for easy data retrieval and manipulation.
    public abstract partial class DatbaseModificationPlugin : BaseUnityPlugin
    {
        protected internal T GetThing<T>(Func<T> func) where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(); }
            catch { return null; }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal T GetThing<T, Ta>(
            Func<Ta, T> func, Ta arg1) 
            where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1); }
            catch { return null; }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal T GetThing<T, Ta, Tb>(
            Func<Ta, Tb, T> func, Ta arg1, Tb arg2)
            where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1, arg2); }
            catch { return null; }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  T GetThing<T, Ta, Tb, Tc>(
            Func<Ta, Tb, Tc, T> func, Ta arg1, Tb arg2, Tc arg3)
            where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1, arg2, arg3); }
            catch { return null; }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  T GetThing<T, Ta, Tb, Tc, Td>(
            Func<Ta, Tb, Tc, Td, T> func, 
            Ta arg1, Tb arg2, Tc arg3, Td arg4)
            where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1, arg2, arg3, arg4); }
            catch { return null; }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  List<T> GetThings<T>(
            Func<List<T>> func)
        where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(); }
            catch { return new List<T>(); }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  List<T> GetThings<T, Ta>(
            Func<Ta, List<T>> func,
            Ta arg1)
        where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1); }
            catch { return new List<T>(); }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  List<T> GetThings<T, Ta, Tb>(
            Func<Ta, Tb, List<T>> func,
            Ta arg1, Tb arg2)
        where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1, arg2); }
            catch { return new List<T>(); }
            finally { FrameworkUtils.CleanMod(); }
        }

        protected internal  List<T> GetThings<T, Ta, Tb, Tc>(
            Func<Ta, Tb, Tc, List<T>> func,
            Ta arg1, Tb arg2, Tc arg3)
        where T : class
        {
            FrameworkUtils.RegisterMod(ModId);
            try { return func(arg1, arg2, arg3); }
            catch { return new List<T>(); }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  void DoThing<Ta, Tb>(
            Action<Ta, Tb> func,
            Ta arg1, Tb arg2)
        {
            FrameworkUtils.RegisterMod(ModId);
            try { func(arg1, arg2); }
            finally { FrameworkUtils.CleanMod(); }
        }
        protected internal  void DoThing<Ta, Tb, Tc>(
            Action<Ta, Tb, Tc> func,
            Ta arg1, Tb arg2, Tc arg3)
        {
            FrameworkUtils.RegisterMod(ModId);
            try { func(arg1, arg2, arg3); }
            finally { FrameworkUtils.CleanMod(); }
        }

    }
}
