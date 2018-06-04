using UnityEditor;

[CustomPropertyDrawer(typeof(StringAudioClipDictionary))]
[CustomPropertyDrawer(typeof(StringTrackGroupDictionary))]
public class CustomSerializableDictionaries : SerializableDictionaryPropertyDrawer { }


