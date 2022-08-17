using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class ParseProfileXML {
        public ParseProfileXML(string text, string profileName) {
            Document = new XmlDocument();
            Document.LoadXml(text);
            Profile = new Profile();
            Profile.Name = profileName;
        }

        public static ParseProfileXML LoadFile(string file) {
            var text = System.IO.File.ReadAllText(System.IO.Path.Combine("Profiles", file));
            return LoadText(text, System.IO.Path.GetFileNameWithoutExtension(file));
        }

        public static ParseProfileXML LoadText(string text, string profileName) {
            return new ParseProfileXML(text, profileName);
        }

        public XmlDocument Document;
        public Profile Profile;

        private readonly List<VariableAssignment> _variableAssignments = new List<VariableAssignment>();
        private class VariableAssignment {
            public object Instance;
            public System.Reflection.PropertyInfo PropertyInfo;
            public string VariableName;
        }

        public object ReflectNode(XmlNode node) {
            Type type;
            if (TypeDictionary.TryGetValue(node.Name, out type) == false)
                type = Type.GetType("PadOS.SaveData.ProfileXML." + node.Name);
            return ReflectNode(node, type);
        }

        public T ReflectNode<T>(XmlNode node) {
            return (T)ReflectNode(node, typeof(T));
        }

        public object ReflectNode(XmlNode node, Type type) {
            if (node.Name == "#comment")
                return null;
            var instance = Activator.CreateInstance(type);
            foreach (var property in type.GetProperties()){
                var attr = node.Attributes[property.Name];
                if (attr == null)
                    continue;
                if (attr.Value.StartsWith("{")){
                    var key = attr.Value.Substring(1, attr.Value.Length - 2);
                    _variableAssignments.Add(new VariableAssignment {
                        Instance = instance,
                        PropertyInfo = property,
                        VariableName = key
                    });
                }
                else if (property.PropertyType == typeof(string))
                    property.SetValue(instance, attr.Value);
                else{
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (converter == null || converter.CanConvertFrom(typeof(string))==false)
                        continue;
                    var value = converter.ConvertFromString(attr.Value);
                    property.SetValue(instance, value);
                }
            }
            if (instance is IParseXML parse)
                parse.Parse(this, node);
            return instance;
        }

        public Profile Parse(){
            var mapping = new Mapping();
            var variables = new Dictionary<string, int>();
            var handlingTrigger = true;
            //Profile.Name = Document.DocumentElement.GetAttribute(nameof(Profile.Name));
            foreach (XmlNode node in Document.DocumentElement.ChildNodes) {
                Type type;
                if (TypeDictionary.TryGetValue(node.Name, out type) == false)
                    type = Type.GetType("PadOS.SaveData.ProfileXML."+node.Name);

                if(type == typeof(Variable)) {
                    var instance = (Variable)ReflectNode(node, type);
                    Profile.Variables.Add(instance);
                    variables[instance.Name] = instance.Value;
                    continue;
                }

                if (type == typeof(Plugin)) {
                    var instance = (Plugin)ReflectNode(node, type);
                    instance.Parse(this, node);
                    Profile.Plugins.Add(instance);
                    continue;
                }

                if (TriggerInterfaceType.IsAssignableFrom(type)) {
                    if (handlingTrigger == false) {
                        Profile.Mappings.Add(mapping);
                        mapping = new Mapping();
                    }
                    handlingTrigger = true;
                    var instance = (ITrigger) ReflectNode(node, type);
                    mapping.Triggers.Add(instance);
                    continue;
                }
                if (ActionInterfaceType.IsAssignableFrom(type)) {
                    handlingTrigger = false;
                    var instance = (IAction) ReflectNode(node, type);
                    mapping.Actions.Add(instance);
                    continue;
                }
            }
            Profile.Mappings.Add(mapping);
            foreach (var item in _variableAssignments) {
                item.PropertyInfo.SetValue(item.Instance, variables[item.VariableName]);
            }
            return Profile;
        }

        private static readonly Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type> {
            {"Variable", typeof(Variable)},
            {"ButtonTrigger", typeof(ButtonTrigger)},
            {"KeyboardAction", typeof(KeyboardAction)},
            {"RepeatAction", typeof(RepeatAction)},
            {"Plugin", typeof(Plugin)},
            {"TriggerPlugin", typeof(TriggerPlugin)},
            {"PadOSAction", typeof(PadOSAction)},
            {"ComboTrigger", typeof(ComboTrigger)},
            {"ActionPlugin", typeof(ActionPlugin)},
            {"SystemAction", typeof(SystemAction)},
            {"SequenceTrigger", typeof(SequenceTrigger)},
            {"PadOSTrigger", typeof(PadOSTrigger)},
        };

        private static readonly Type TriggerInterfaceType = typeof(ITrigger);
        private static readonly Type ActionInterfaceType = typeof(IAction);
    }
}
