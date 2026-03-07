using Mono.Cecil;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using static Mono.Security.X509.X520;

namespace Database_Modification_Framework.Definitions
{
    public interface IGAttribute
    {
        Type ValueType { get; }
        object GetValue();
        string GetRawString();
        string ToGlobalAttributeLine();
        string SourceString { get; }
        string Name { get; }
    }
    public interface IGAttribute<T> : IGAttribute
    {
        T Value { get; }
    }
    public abstract class ParentAttribute<T> : IGAttribute<T>
    {
        private string _sourceString;
        public string SourceString => _sourceString;
        public string Name { get; }
        internal string _value { get; set; }
        public virtual T Value { get; set; }
        public Type ValueType { get => typeof(T); }
        public object GetValue() => Value;
        public string GetRawString() => _value;
        public string ToGlobalAttributeLine() => $"{Name}={_value}";
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
        public SpecialNumeric(string gaLine) : base(gaLine) { }
        public SpecialNumeric(string name, int value) : base(name, value) { }
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
        public StringBool(string gaLine) : base(gaLine) { }
        public StringBool(string name, bool value) : base(name, value) { }
    }
    public class NumericBool : ParentAttribute<bool>
    {
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
        public NumericBool(string gaLine) : base(gaLine) { }
        public NumericBool(string name, bool value) : base(name, value) { }
    }
}


