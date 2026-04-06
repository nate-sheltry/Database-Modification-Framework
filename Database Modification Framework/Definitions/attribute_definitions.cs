using BepInEx;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Diagnostics;
using static Database_Modification_Framework.FrameworkUtils;


namespace Database_Modification_Framework.Definitions
{
    public interface IGAttribute
    {
        Type ValueType { get; }
        object GetValue();
        object GetRuntimeValue();
        Type AttributeType { get; }
        string GetRawString();
        string ToGlobalAttributeLine();
        ItemAttribute ToItemAttribute();
        string SourceString { get; }
        string Name { get; }
    }
    public interface IGAttribute<T> : IGAttribute
    {
        T Value { get; }
    }
    public abstract class ParentAttribute<T> : IGAttribute<T>
    {
        protected internal string _sourceString;
        public virtual Type AttributeType { get => this.GetType(); }
        public string SourceString => _sourceString;
        public string Name { get; }
        internal string _value { get; set; }
        public virtual T Value { get; set; }
        public Type ValueType { get => typeof(T); }
        public object GetValue() => Value;
        public virtual object GetRuntimeValue() => Value;
        public string GetRawString() => _value;
        public string ToGlobalAttributeLine() => $"{Name}={_value}";
        public ItemAttribute ToItemAttribute()
        {
            return new ItemAttribute(Name, GetRuntimeValue());
        }
        public ParentAttribute(object data)
        {
            this._sourceString = GetPropValue<string>(data, "_sourceString");
            this.Name = GetPropValue<string>(data, "Name");
            this._value = GetPropValue<string>(data, "_value");
        }
        public ParentAttribute(string gaLine)
        {
            this._sourceString = gaLine;
            string[] parts = gaLine.Split('=');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid attribute line.");
            (this.Name, this._value) = (parts[0], parts[1]);
        }
        public ParentAttribute(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class GenericAttribute : ParentAttribute<string>
    {
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
        public override object GetRuntimeValue()
        {
            if(_value.Count(c => c == '\"') == 2)
            {
                int length = _value.Length;
                if (_value[0] == '\"' && _value[length-1] == '\"')
                {
                    return _value.Remove(length-1, 1).Remove(0, 1);
                }
            }
            return _value;
        }
        public GenericAttribute(object data) : base(data) { }
        public GenericAttribute(string gaLine) : base(gaLine) { }
        public GenericAttribute(string name, string value) : base(name, value) { }
    }

    public class SpecialNumeric : ParentAttribute<int>
    {

        public override int Value
        {
            get
            {
                int rawInt;
                if (int.TryParse(_value.Remove(0, 1), out rawInt))
                    return rawInt;
                throw new ArgumentNullException($"Failed to parse {Name} value.");
            }
            set
            {
                if (value < 0)
                    throw new ArgumentNullException($"{Name} attribute cannot be less than 0.");
                _value = $"d{value}";
            }
        }
        public SpecialNumeric(object data) : base(data) { }
        public SpecialNumeric(string gaLine) : base(gaLine) { }
        public SpecialNumeric(string name, int value) : base(name, value) { }
    }

    public class RepairNumeric : ParentAttribute<int>
    {
        public override int Value
        {
            get
            {
                int rawInt;
                if (int.TryParse(_value.Remove(0, 1), out rawInt))
                    return rawInt;
                throw new ArgumentNullException($"Failed to parse {Name} value.");
            }
            set
            {
                if (value < 0)
                    throw new ArgumentNullException($"{Name} attribute cannot be less than 0.");
                _value = $"p{value}";
            }
        }
        public RepairNumeric(object data) : base(data) { }
        public RepairNumeric(string gaLine) : base(gaLine) { }
        public RepairNumeric(string name, int value) : base(name, value) { }
    }

    public class GenericFloat : ParentAttribute<float>
    {
        public override float Value
        {
            get
            {
                float rawFloat;
                if (float.TryParse(_value, out rawFloat))
                    return rawFloat;
                throw new ArgumentException($"Failed to parse {Name} value.");
            }
            set
            {
                if (value < 0)
                    throw new ArgumentNullException($"{Name} attribute cannot be less than 0.");
                _value = $"{value}";
            }
        }
        public GenericFloat(object data) : base(data) { }
        public GenericFloat(string gaLine) : base(gaLine) { }
        public GenericFloat(string name, float value) : base(name, value) { }
    }

    public class GenericInt : ParentAttribute<int>
    {
        public override int Value
        {
            get
            {
                int rawInt;
                if (int.TryParse(_value, out rawInt))
                    return rawInt;
                throw new ArgumentException($"Failed to parse {Name} value.");
            }
            set
            {
                if (value < 0)
                    throw new ArgumentNullException($"{Name} attribute cannot be less than 0.");
                _value = $"{value}";
            }
        }
        public GenericInt(object data) : base(data) { }
        public GenericInt(string gaLine) : base(gaLine) { }
        public GenericInt(string name, int value) : base(name, value) { }
    }

    public class StringBool : ParentAttribute<bool>
    {
        public override bool Value
        {
            get
            {
                bool rawBool;
                if (bool.TryParse(_value.Remove(0, 1), out rawBool))
                    return rawBool;
                throw new ArgumentException($"Failed to parse {Name} value.");
            }
            set
            {
                _value = $"#{value.ToString().ToLower()}";
            }
        }
        public StringBool(object data) : base(data) { }
        public StringBool(string gaLine) : base(gaLine) { }
        public StringBool(string name, bool value) : base(name, value) { }
    }
    public class NumericBool : ParentAttribute<bool>
    {
        public override Type AttributeType { get => typeof(NumericBool); }
        public override bool Value
        {
            get
            {
                int rawInt;
                if (int.TryParse(_value, out rawInt))
                    return rawInt > 0;
                throw new ArgumentException($"Failed to parse {Name} value.");
            }
            set
            {
                _value = value ? "1" : "0";
            }
        }
        public NumericBool(object data) : base(data) { }
        public NumericBool(string gaLine) : base(gaLine) { }
        public NumericBool(string name, bool value) : base(name, value) { }
    }
    public static class AttributeReader
    {
        // Performance could be better here, so this will need to be revisited at some point.
        internal static readonly HashSet<string> stringBools = new HashSet<string>
        {
            "_ReloadToUnjam", "_NoHeadShots", "_Cookable",
            "_IsRanged", "_hideHats", "_IsFull",
        };

        internal static readonly HashSet<string> numericBools = new HashSet<string>
        {
            "_AlwaysKnockBack", "_IsGrenade", "_IsIllustration",
            "_convertsDamage", "_returnsDamage", "_removeSpeedPenalty",
            "_NoHelmet", "_IsHeavy", "_NoJump",
            "_nightVision", "_NoSprint", "_boostArmStrength",
            "_MaxMoveSpeed",
        };

        internal static readonly HashSet<string> genericInts = new HashSet<string>
        {
            "_Radiation","_PoisonDuration","_CausticDuration",
            "_visitationProtection","_gasProtection","_Toxin",
            "_Length","Blunt Damage","_DamageHigh",
            "Padding","_AccuracyMult","_Caustic",
            "_PoisonDamage","Poison Hits","Radiation",
            "_StaminaRestoreBoost","Armor","Charges",
            "_LengthSec","_Duration","_MoreCarryWeight",
            "_NoiseLevel","_Damage","Radius",
            "_Muzzle Velocity","Penetration","_ExtraRange",
            "Capacity","Electrical","Calories",
            "_carryWeight","_Electric","Sharp Damage",
        };

        internal static readonly HashSet<string> genericFloats = new HashSet<string>
        {
            "_Rarity","Rate of Fire","_reduceNoise",
            "_reduceNightVisibility","Battery Drain","_staminaConversion",
            "Swing Speed","_RequireStrength","_Bleeding",
            "Recoil","_DamageDrop","_restoreHealth",
            "Accuracy","Detection Radius","Speed",
            "Handling","Coverage","_ReloadSpeed",
            "_Encumbrance","Growth Period","Damage",
        };

        internal static readonly HashSet<string> specialNumerics = new HashSet<string>
        {
            "Pockets", "_NewRows", "_CurrentFireMode",
            "Magazine Size", "_numberOfProjectiles",
            "_ThrowType", "_LoadedAmmos",
        };

        public static IGAttribute ReadAttribute(string line)
        {
            string[] breakdown = line.Split('=');
            if(breakdown.Length == 1)
            {
                FrameworkUtils.InternalLog(BepInEx.Logging.LogLevel.Debug,
                    $"Generic Attribute with null value was made."
                );
                return new GenericAttribute(line, null);
            }
            string key, value;
            (key, value) = (breakdown[0], breakdown[1]);
            if (key.Contains("Repair Amount"))
                return new RepairNumeric(line);
            else if (stringBools.Contains(key))
                return new StringBool(line);
            else if (numericBools.Contains(key))
                return new NumericBool(line);
            else if (genericInts.Contains(key))
                return new GenericInt(line);
            else if (genericFloats.Contains(key))
                return new GenericFloat(line);
            else if (specialNumerics.Contains(key))
                return new SpecialNumeric(line);
            else
                return new GenericAttribute(line);
        }
    }
}


