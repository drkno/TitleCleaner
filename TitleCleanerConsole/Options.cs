/*
 * This file is a modified version of Mono.Options/NDesk.Options.
 * Origionally taken from https://github.com/mono/mono/blob/master/mcs/class/Mono.Options/Mono.Options/Options.cs
 * As of the copy being taken (08/10/14) the license was compatible with the MIT license.
 */

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace TitleCleanerConsole
{
    public class OptionValueCollection : IList, IList<string>
    {
        private readonly OptionContext _c;
        private readonly List<string> _values = new List<string>();

        internal OptionValueCollection(OptionContext c)
        {
            _c = c;
        }

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            (_values as ICollection).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return (_values as ICollection).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return (_values as ICollection).SyncRoot; }
        }

        #endregion

        #region ICollection<T>

        public void Clear()
        {
            _values.Clear();
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(string item)
        {
            _values.Add(item);
        }

        public bool Contains(string item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _values.Remove(item);
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<string> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        #endregion

        #region IList

        int IList.Add(object value)
        {
            return (_values as IList).Add(value);
        }

        bool IList.Contains(object value)
        {
            return (_values as IList).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return (_values as IList).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            (_values as IList).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            (_values as IList).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            (_values as IList).RemoveAt(index);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { (_values as IList)[index] = value; }
        }

        #endregion

        #region IList<T>

        public int IndexOf(string item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                AssertValid(index);
                return index >= _values.Count ? null : _values[index];
            }
            set { _values[index] = value; }
        }

        private void AssertValid(int index)
        {
            if (_c.Option == null)
                throw new InvalidOperationException("OptionContext.Option is null.");
            if (index >= _c.Option.MaxValueCount)
                throw new ArgumentOutOfRangeException("index");
            if (_c.Option.OptionValueType == OptionValueType.Required &&
                index >= _values.Count)
                throw new OptionException(string.Format(
                    _c.OptionSet.MessageLocalizer("Missing required value for option '{0}'."), _c.OptionName),
                    _c.OptionName);
        }

        #endregion

        public List<string> ToList()
        {
            return new List<string>(_values);
        }

        public string[] ToArray()
        {
            return _values.ToArray();
        }

        public override string ToString()
        {
            return string.Join(", ", _values.ToArray());
        }
    }

    public class OptionContext
    {
        private readonly OptionValueCollection _c;
        private readonly OptionSet _set;

        public OptionContext(OptionSet set)
        {
            _set = set;
            _c = new OptionValueCollection(this);
        }

        public Option Option { get; set; }

        public string OptionName { get; set; }

        public int OptionIndex { get; set; }

        public OptionSet OptionSet
        {
            get { return _set; }
        }

        public OptionValueCollection OptionValues
        {
            get { return _c; }
        }
    }

    public enum OptionValueType
    {
        None,
        Optional,
        Required,
    }

    public abstract class Option
    {
        private static readonly char[] NameTerminator = {'=', ':'};
        private readonly int _count;
        private readonly string _description;
        private readonly string[] _names;
        private readonly string _prototype;
        private readonly OptionValueType _type;
        private string[] _separators;

        protected Option(string prototype, string description)
            : this(prototype, description, 1)
        {
        }

        protected Option(string prototype, string description, int maxValueCount)
        {
            if (prototype == null)
                throw new ArgumentNullException("prototype");
            if (prototype.Length == 0)
                throw new ArgumentException("Cannot be the empty string.", "prototype");
            if (maxValueCount < 0)
                throw new ArgumentOutOfRangeException("maxValueCount");

            _prototype = prototype;
            _names = prototype.Split('|');
            _description = description;
            _count = maxValueCount;
            _type = ParsePrototype();

            if (_count == 0 && _type != OptionValueType.None)
                throw new ArgumentException(
                    "Cannot provide maxValueCount of 0 for OptionValueType.Required or " +
                    "OptionValueType.Optional.",
                    "maxValueCount");
            if (_type == OptionValueType.None && maxValueCount > 1)
                throw new ArgumentException(
                    string.Format("Cannot provide maxValueCount of {0} for OptionValueType.None.", maxValueCount),
                    "maxValueCount");
            if (Array.IndexOf(_names, "<>") >= 0 &&
                ((_names.Length == 1 && _type != OptionValueType.None) ||
                 (_names.Length > 1 && MaxValueCount > 1)))
                throw new ArgumentException(
                    "The default option handler '<>' cannot require values.",
                    "prototype");
        }

        public string Prototype
        {
            get { return _prototype; }
        }

        public string Description
        {
            get { return _description; }
        }

        public OptionValueType OptionValueType
        {
            get { return _type; }
        }

        public int MaxValueCount
        {
            get { return _count; }
        }

        internal string[] Names
        {
            get { return _names; }
        }

        internal string[] ValueSeparators
        {
            get { return _separators; }
        }

        public string[] GetNames()
        {
            return (string[]) _names.Clone();
        }

        public string[] GetValueSeparators()
        {
            if (_separators == null)
                return new string[0];
            return (string[]) _separators.Clone();
        }

        protected static T Parse<T>(string value, OptionContext c)
        {
            var conv = TypeDescriptor.GetConverter(typeof (T));
            var t = default(T);
            try
            {
                if (value != null)
                    t = (T) conv.ConvertFromString(value);
            }
            catch (Exception e)
            {
                throw new OptionException(
                    string.Format(
                        c.OptionSet.MessageLocalizer("Could not convert string `{0}' to type {1} for option `{2}'."),
                        value, typeof (T).Name, c.OptionName),
                    c.OptionName, e);
            }
            return t;
        }

        private OptionValueType ParsePrototype()
        {
            var type = '\0';
            var seps = new List<string>();
            for (var i = 0; i < _names.Length; ++i)
            {
                var name = _names[i];
                if (name.Length == 0)
                    throw new ArgumentException("Empty option names are not supported.", "pro" + "totype");

                var end = name.IndexOfAny(NameTerminator);
                if (end == -1)
                    continue;
                _names[i] = name.Substring(0, end);
                if (type == '\0' || type == name[end])
                    type = name[end];
                else
                    throw new ArgumentException(
                        string.Format("Conflicting option types: '{0}' vs. '{1}'.", type, name[end]),
                        "prot" + "otype");
                AddSeparators(name, end, seps);
            }

            if (type == '\0')
                return OptionValueType.None;

            if (_count <= 1 && seps.Count != 0)
                throw new ArgumentException(
                    string.Format("Cannot provide key/value separators for Options taking {0} value(s).", _count),
                    "protot" + "ype");
            if (_count > 1)
            {
                if (seps.Count == 0)
                    _separators = new[] {":", "="};
                else if (seps.Count == 1 && seps[0].Length == 0)
                    _separators = null;
                else
                    _separators = seps.ToArray();
            }

            return type == '=' ? OptionValueType.Required : OptionValueType.Optional;
        }

        private static void AddSeparators(string name, int end, ICollection<string> seps)
        {
            var start = -1;
            for (var i = end + 1; i < name.Length; ++i)
            {
                switch (name[i])
                {
                    case '{':
                        if (start != -1)
                            throw new ArgumentException(
                                string.Format("Ill-formed name/value separator found in \"{0}\".", name),
                                "name");
                        start = i + 1;
                        break;
                    case '}':
                        if (start == -1)
                            throw new ArgumentException(
                                string.Format("Ill-formed name/value separator found in \"{0}\".", name),
                                "name");
                        seps.Add(name.Substring(start, i - start));
                        start = -1;
                        break;
                    default:
                        if (start == -1)
                            seps.Add(name[i].ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }
            if (start != -1)
                throw new ArgumentException(
                    string.Format("Ill-formed name/value separator found in \"{0}\".", name),
                    "name");
        }

        public void Invoke(OptionContext c)
        {
            OnParseComplete(c);
            c.OptionName = null;
            c.Option = null;
            c.OptionValues.Clear();
        }

        protected abstract void OnParseComplete(OptionContext c);

        public override string ToString()
        {
            return Prototype;
        }
    }

    [Serializable]
    public class OptionException : Exception
    {
        private readonly string _option;

        public OptionException()
        {
        }

        public OptionException(string message, string optionName)
            : base(message)
        {
            _option = optionName;
        }

        public OptionException(string message, string optionName, Exception innerException)
            : base(message, innerException)
        {
            _option = optionName;
        }

        protected OptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _option = info.GetString("OptionName");
        }

        public string OptionName
        {
            get { return _option; }
        }

        [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("OptionName", _option);
        }
    }

    public delegate void OptionAction<in TKey, in TValue>(TKey key, TValue value);

    public class OptionSet : KeyedCollection<string, Option>
    {
        public OptionSet()
            : this(f => f)
        {
        }

        public OptionSet(Converter<string, string> localizer)
        {
            _localizer = localizer;
        }

        private readonly Converter<string, string> _localizer;

        public Converter<string, string> MessageLocalizer
        {
            get { return _localizer; }
        }

        protected override string GetKeyForItem(Option item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (item.Names != null && item.Names.Length > 0)
                return item.Names[0];
            // This should never happen, as it's invalid for Option to be
            // constructed w/o any names.
            throw new InvalidOperationException("Option has no names!");
        }

        [Obsolete("Use KeyedCollection.this[string]")]
        protected Option GetOptionForName(string option)
        {
            if (option == null)
                throw new ArgumentNullException("option");
            try
            {
                return base[option];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        protected override void InsertItem(int index, Option item)
        {
            base.InsertItem(index, item);
            AddImpl(item);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            var p = Items[index];
            // KeyedCollection.RemoveItem() handles the 0th item
            for (var i = 1; i < p.Names.Length; ++i)
            {
                Dictionary.Remove(p.Names[i]);
            }
        }

        protected override void SetItem(int index, Option item)
        {
            base.SetItem(index, item);
            RemoveItem(index);
            AddImpl(item);
        }

        private void AddImpl(Option option)
        {
            if (option == null)
                throw new ArgumentNullException("option");
            var added = new List<string>(option.Names.Length);
            try
            {
                // KeyedCollection.InsertItem/SetItem handle the 0th name.
                for (var i = 1; i < option.Names.Length; ++i)
                {
                    Dictionary.Add(option.Names[i], option);
                    added.Add(option.Names[i]);
                }
            }
            catch (Exception)
            {
                foreach (var name in added)
                    Dictionary.Remove(name);
                throw;
            }
        }

        public new OptionSet Add(Option option)
        {
            base.Add(option);
            return this;
        }

        private sealed class ActionOption : Option
        {
            private readonly Action<OptionValueCollection> _action;

            public ActionOption(string prototype, string description, int count, Action<OptionValueCollection> action)
                : base(prototype, description, count)
            {
                if (action == null)
                    throw new ArgumentNullException("action");
                _action = action;
            }

            protected override void OnParseComplete(OptionContext c)
            {
                _action(c.OptionValues);
            }
        }

        public OptionSet Add(string prototype, Action<string> action)
        {
            return Add(prototype, null, action);
        }

        public OptionSet Add(string prototype, string description, Action<string> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            Option p = new ActionOption(prototype, description, 1,
                v => action(v[0]));
            base.Add(p);
            return this;
        }

        public OptionSet Add(string prototype, OptionAction<string, string> action)
        {
            return Add(prototype, null, action);
        }

        public OptionSet Add(string prototype, string description, OptionAction<string, string> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            Option p = new ActionOption(prototype, description, 2,
                v => action(v[0], v[1]));
            base.Add(p);
            return this;
        }

        private sealed class ActionOption<T> : Option
        {
            private readonly Action<T> _action;

            public ActionOption(string prototype, string description, Action<T> action)
                : base(prototype, description, 1)
            {
                if (action == null)
                    throw new ArgumentNullException("action");
                _action = action;
            }

            protected override void OnParseComplete(OptionContext c)
            {
                _action(Parse<T>(c.OptionValues[0], c));
            }
        }

        private sealed class ActionOption<TKey, TValue> : Option
        {
            private readonly OptionAction<TKey, TValue> _action;

            public ActionOption(string prototype, string description, OptionAction<TKey, TValue> action)
                : base(prototype, description, 2)
            {
                if (action == null)
                    throw new ArgumentNullException("action");
                _action = action;
            }

            protected override void OnParseComplete(OptionContext c)
            {
                _action(
                    Parse<TKey>(c.OptionValues[0], c),
                    Parse<TValue>(c.OptionValues[1], c));
            }
        }

        public OptionSet Add<T>(string prototype, Action<T> action)
        {
            return Add(prototype, null, action);
        }

        public OptionSet Add<T>(string prototype, string description, Action<T> action)
        {
            return Add(new ActionOption<T>(prototype, description, action));
        }

        public OptionSet Add<TKey, TValue>(string prototype, OptionAction<TKey, TValue> action)
        {
            return Add(prototype, null, action);
        }

        public OptionSet Add<TKey, TValue>(string prototype, string description, OptionAction<TKey, TValue> action)
        {
            return Add(new ActionOption<TKey, TValue>(prototype, description, action));
        }

        protected virtual OptionContext CreateOptionContext()
        {
            return new OptionContext(this);
        }

        public List<string> Parse(IEnumerable<string> arguments)
        {
            var c = CreateOptionContext();
            c.OptionIndex = -1;
            var process = true;
            var unprocessed = new List<string>();
            var def = Contains("<>") ? this["<>"] : null;
            foreach (var argument in arguments)
            {
                ++c.OptionIndex;
                if (argument == "--")
                {
                    process = false;
                    continue;
                }
                if (!process)
                {
                    Unprocessed(unprocessed, def, c, argument);
                    continue;
                }
                if (!Parse(argument, c))
                    Unprocessed(unprocessed, def, c, argument);
            }
            if (c.Option != null)
                c.Option.Invoke(c);
            return unprocessed;
        }

        public void ParseExceptionally(IEnumerable<string> arguments)
        {
            var result = Parse(arguments);
            if (result.Count <= 0) return;
            var options = "";
            options = result.Aggregate(options, (current, r) => current + (" " + r));
            throw new OptionException("Unknown option" + (result.Count > 1 ? "s" : "") + " " + options, null);
        }

        private static void Unprocessed(ICollection<string> extra, Option def, OptionContext c, string argument)
        {
            if (def == null)
            {
                extra.Add(argument);
                return;
            }
            c.OptionValues.Add(argument);
            c.Option = def;
            c.Option.Invoke(c);
        }

        private readonly Regex _valueOption = new Regex(
            @"^(?<flag>--|-|/)(?<name>[^:=]+)((?<sep>[:=])(?<value>.*))?$");

        protected bool GetOptionParts(string argument, out string flag, out string name, out string sep,
            out string value)
        {
            if (argument == null)
                throw new ArgumentNullException("argument");

            flag = name = sep = value = null;
            var m = _valueOption.Match(argument);
            if (!m.Success)
            {
                return false;
            }
            flag = m.Groups["flag"].Value;
            name = m.Groups["name"].Value;
            if (m.Groups["sep"].Success && m.Groups["value"].Success)
            {
                sep = m.Groups["sep"].Value;
                value = m.Groups["value"].Value;
            }
            return true;
        }

        protected virtual bool Parse(string argument, OptionContext c)
        {
            if (c.Option != null)
            {
                ParseValue(argument, c);
                return true;
            }

            string f, n, s, v;
            if (!GetOptionParts(argument, out f, out n, out s, out v))
                return false;

            if (!Contains(n)) return ParseBool(argument, n, c) || ParseBundledValue(f, n + s + v, c);
            var p = this[n];
            c.OptionName = f + n;
            c.Option = p;
            switch (p.OptionValueType)
            {
                case OptionValueType.None:
                    c.OptionValues.Add(n);
                    c.Option.Invoke(c);
                    break;
                case OptionValueType.Optional:
                case OptionValueType.Required:
                    ParseValue(v, c);
                    break;
            }
            return true;
            // no match; is it a bool option?
            // is it a bundled option?
        }

        private void ParseValue(string option, OptionContext c)
        {
            if (option != null)
                foreach (var o in c.Option.ValueSeparators != null
                    ? option.Split(c.Option.ValueSeparators, StringSplitOptions.None)
                    : new[] {option})
                {
                    c.OptionValues.Add(o);
                }
            if (c.OptionValues.Count == c.Option.MaxValueCount ||
                c.Option.OptionValueType == OptionValueType.Optional)
                c.Option.Invoke(c);
            else if (c.OptionValues.Count > c.Option.MaxValueCount)
            {
                throw new OptionException(_localizer(string.Format(
                    "Error: Found {0} option values when expecting {1}.",
                    c.OptionValues.Count, c.Option.MaxValueCount)),
                    c.OptionName);
            }
        }

        private bool ParseBool(string option, string n, OptionContext c)
        {
            string rn;
            if (n.Length < 1 || (n[n.Length - 1] != '+' && n[n.Length - 1] != '-') ||
                !Contains((rn = n.Substring(0, n.Length - 1)))) return false;
            var p = this[rn];
            var v = n[n.Length - 1] == '+' ? option : null;
            c.OptionName = option;
            c.Option = p;
            c.OptionValues.Add(v);
            p.Invoke(c);
            return true;
        }

        private bool ParseBundledValue(string f, string n, OptionContext c)
        {
            if (f != "-")
                return false;
            for (var i = 0; i < n.Length; ++i)
            {
                var opt = f + n[i];
                var rn = n[i].ToString(CultureInfo.InvariantCulture);
                if (!Contains(rn))
                {
                    if (i == 0)
                        return false;
                    throw new OptionException(string.Format(_localizer(
                        "Cannot bundle unregistered option '{0}'."), opt), opt);
                }
                var p = this[rn];
                switch (p.OptionValueType)
                {
                    case OptionValueType.None:
                        Invoke(c, opt, n, p);
                        break;
                    case OptionValueType.Optional:
                    case OptionValueType.Required:
                    {
                        var v = n.Substring(i + 1);
                        c.Option = p;
                        c.OptionName = opt;
                        ParseValue(v.Length != 0 ? v : null, c);
                        return true;
                    }
                    default:
                        throw new InvalidOperationException("Unknown OptionValueType: " + p.OptionValueType);
                }
            }
            return true;
        }

        private static void Invoke(OptionContext c, string name, string value, Option option)
        {
            c.OptionName = name;
            c.Option = option;
            c.OptionValues.Add(value);
            option.Invoke(c);
        }

        private const int OptionWidth = 30;

        public void WriteOptionDescriptions(TextWriter o)
        {
            foreach (var p in this)
            {
                var written = 0;
                if (!WriteOptionPrototype(o, p, ref written))
                    continue;

                if (written < OptionWidth)
                    o.Write(new string(' ', OptionWidth - written));
                else
                {
                    o.WriteLine();
                    o.Write(new string(' ', OptionWidth));
                }

                var lines = GetLines(_localizer(GetDescription(p.Description)));
                o.WriteLine(lines[0]);
                var prefix = new string(' ', OptionWidth + 2);
                for (var i = 1; i < lines.Count; ++i)
                {
                    o.Write(prefix);
                    o.WriteLine(lines[i]);
                }
            }
        }

        private bool WriteOptionPrototype(TextWriter o, Option p, ref int written)
        {
            var names = p.Names;

            var i = GetNextOptionIndex(names, 0);
            if (i == names.Length)
                return false;

            if (names[i].Length == 1)
            {
                Write(o, ref written, "\t-");   // " -"); // changed for tab instead of space indent
                Write(o, ref written, names[0]);
            }
            else
            {
                Write(o, ref written, "      --");
                Write(o, ref written, names[0]);
            }

            for (i = GetNextOptionIndex(names, i + 1);
                i < names.Length;
                i = GetNextOptionIndex(names, i + 1))
            {
                Write(o, ref written, ", ");
                Write(o, ref written, names[i].Length == 1 ? "-" : "--");
                Write(o, ref written, names[i]);
            }

            if (p.OptionValueType == OptionValueType.Optional ||
                p.OptionValueType == OptionValueType.Required)
            {
                if (p.OptionValueType == OptionValueType.Optional)
                {
                    Write(o, ref written, _localizer("["));
                }
                Write(o, ref written, _localizer("=" + GetArgumentName(0, p.MaxValueCount, p.Description)));
                var sep = p.ValueSeparators != null && p.ValueSeparators.Length > 0
                    ? p.ValueSeparators[0]
                    : " ";
                for (var c = 1; c < p.MaxValueCount; ++c)
                {
                    Write(o, ref written, _localizer(sep + GetArgumentName(c, p.MaxValueCount, p.Description)));
                }
                if (p.OptionValueType == OptionValueType.Optional)
                {
                    Write(o, ref written, _localizer("]"));
                }
            }
            return true;
        }

        private static int GetNextOptionIndex(string[] names, int i)
        {
            while (i < names.Length && names[i] == "<>")
            {
                ++i;
            }
            return i;
        }

        private static void Write(TextWriter o, ref int n, string s)
        {
            n += s.Length;
            o.Write(s);
        }

        private static string GetArgumentName(int index, int maxIndex, string description)
        {
            if (description == null)
                return maxIndex == 1 ? "VALUE" : "VALUE" + (index + 1);
            var nameStart = maxIndex == 1 ? new[] {"{0:", "{"} : new[] {"{" + index + ":"};
            foreach (var t in nameStart)
            {
                int start, j = 0;
                do
                {
                    start = description.IndexOf(t, j, StringComparison.Ordinal);
                } while (start >= 0 && j != 0 && description[j++ - 1] == '{');
                if (start == -1)
                    continue;
                var end = description.IndexOf("}", start, StringComparison.Ordinal);
                if (end == -1)
                    continue;
                return description.Substring(start + t.Length, end - start - t.Length);
            }
            return maxIndex == 1 ? "VALUE" : "VALUE" + (index + 1);
        }

        private static string GetDescription(string description)
        {
            if (description == null)
                return string.Empty;
            var sb = new StringBuilder(description.Length);
            var start = -1;
            for (var i = 0; i < description.Length; ++i)
            {
                switch (description[i])
                {
                    case '{':
                        if (i == start)
                        {
                            sb.Append('{');
                            start = -1;
                        }
                        else if (start < 0)
                            start = i + 1;
                        break;
                    case '}':
                        if (start < 0)
                        {
                            if ((i + 1) == description.Length || description[i + 1] != '}')
                                throw new InvalidOperationException("Invalid option description: " + description);
                            ++i;
                            sb.Append("}");
                        }
                        else
                        {
                            sb.Append(description.Substring(start, i - start));
                            start = -1;
                        }
                        break;
                    case ':':
                        if (start < 0)
                            goto default;
                        start = i + 1;
                        break;
                    default:
                        if (start < 0)
                            sb.Append(description[i]);
                        break;
                }
            }
            return sb.ToString();
        }

        private static List<string> GetLines(string description)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                lines.Add(string.Empty);
                return lines;
            }
            const int length = 80 - OptionWidth - 2;
            int start = 0, end;
            do
            {
                end = GetLineEnd(start, length, description);
                var cont = false;
                if (end < description.Length)
                {
                    var c = description[end];
                    if (c == '-' || (char.IsWhiteSpace(c) && c != '\n'))
                        ++end;
                    else if (c != '\n')
                    {
                        cont = true;
                        --end;
                    }
                }
                lines.Add(description.Substring(start, end - start));
                if (cont)
                {
                    lines[lines.Count - 1] += "-";
                }
                start = end;
                if (start < description.Length && description[start] == '\n')
                    ++start;
            } while (end < description.Length);
            return lines;
        }

        private static int GetLineEnd(int start, int length, string description)
        {
            var end = Math.Min(start + length, description.Length);
            var sep = -1;
            for (var i = start; i < end; ++i)
            {
                switch (description[i])
                {
                    case ' ':
                    case '\t':
                    case '\v':
                    case '-':
                    case ',':
                    case '.':
                    case ';':
                        sep = i;
                        break;
                    case '\n':
                        return i;
                }
            }
            if (sep == -1 || end == description.Length)
                return end;
            return sep;
        }

        public void WriteProgramName(string description)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("NAME");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine('\t' + appName + " - " + description + '\n');
            Console.ForegroundColor = origColour;
        }

        public void WriteProgramSynopsis(string synopsis)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SYNOPSIS");
            Console.ForegroundColor = ConsoleColor.Gray;
            synopsis = synopsis.Replace("{appName}", appName);
            Console.WriteLine('\t' + synopsis + '\n');
            Console.ForegroundColor = origColour;
        }

        public void WriteProgramAuthor(string authorByString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("AUTHOR");
            Console.ForegroundColor = ConsoleColor.Gray;
            authorByString = authorByString.Replace("{appName}", appName);
            Console.WriteLine('\t' + authorByString + '\n');
            Console.ForegroundColor = origColour;
        }

        public void WriteProgramReportingBugs(string reportString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("REPORTING BUGS");
            Console.ForegroundColor = ConsoleColor.Gray;
            reportString = reportString.Replace("{appName}", appName);
            var spl = reportString.Split(new []{"\n", "\r\n", "\r"},StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        public void WriteProgramCopyrightLicense(string copyrightLicense)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("COPYRIGHT");
            Console.ForegroundColor = ConsoleColor.Gray;
            copyrightLicense = copyrightLicense.Replace("{appName}", appName);
            var spl = copyrightLicense.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        public void WriteOptionDescriptions(string prefixText, string postText)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("DESCRIPTION");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrWhiteSpace(prefixText))
            {
                prefixText = prefixText.Replace("{appName}", appName);
                var spl = prefixText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }

            var buffWid = Console.BufferWidth;
            foreach (var p in this)
            {
                Console.Write('\t');
                for (var j = 0; j < p.Names.Length; j++)
                {
                    Console.Write(p.Names[j].Length > 1 ? "--" : "-");
                    Console.Write(p.Names[j]);
                    if (j + 1 != p.Names.Length)
                    {
                        Console.Write(", ");
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }

                Console.Write("\t\t");
                var len = buffWid - Console.CursorLeft;
                
                foreach (var l in p.Description.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var lenP = 0;
                    foreach (var w in l.Split(' '))
                    {
                        var word = w;
                        if (lenP != 0)
                        {
                            word = ' ' + word;
                        }
                        if (lenP + word.Length > len)
                        {
                            Console.Write("\n\t\t");
                            lenP = 0;
                        }
                        Console.Write(word);
                        lenP += word.Length;
                    }
                    Console.Write("\n\t\t");
                }
                Console.WriteLine();
            }

            if (!string.IsNullOrWhiteSpace(postText))
            {
                postText = postText.Replace("{appName}", appName);
                var spl = postText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }
    }
}