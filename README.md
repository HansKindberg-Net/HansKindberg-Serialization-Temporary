HansKindberg-Serialization
==========================

Original code/idea
------------------
* [**Anonymous Method Serialization**, by Fredrik Norén](http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx)

Notes
-----
### Serializing large objects
When serializing large objects using HansKindberg.Serialization.Serializable&lt;T&gt; you can get the following exception if the object you want to serialize is very large:
<pre>
System.Runtime.Serialization.SerializationException: The internal array cannot expand to greater than Int32.MaxValue elements.
Result StackTrace:	
at System.Runtime.Serialization.ObjectIDGenerator.Rehash()
   at System.Runtime.Serialization.ObjectIDGenerator.GetId(Object obj, Boolean& firstTime)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.InternalGetId(Object obj, Boolean assignUniqueIdToValueType, Type type, Boolean& isNew)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.Schedule(Object obj, Boolean assignUniqueIdToValueType, Type type, WriteObjectInfo objectInfo)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.WriteArrayMember(WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, Object data)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.WriteArray(WriteObjectInfo objectInfo, NameInfo memberNameInfo, WriteObjectInfo memberObjectInfo)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo)
   at System.Runtime.Serialization.Formatters.Binary.ObjectWriter.Serialize(Object graph, Header[] inHeaders, __BinaryWriter serWriter, Boolean fCheck)
   at System.Runtime.Serialization.Formatters.Binary.BinaryFormatter.Serialize(Stream serializationStream, Object graph, Header[] headers, Boolean fCheck)
   at System.Runtime.Serialization.Formatters.Binary.BinaryFormatter.Serialize(Stream serializationStream, Object graph)
</pre>

* [**Binary serialization fails for moderately large object graphs**](http://connect.microsoft.com/VisualStudio/feedback/details/303278/binary-serialization-fails-for-moderately-large-object-graphs)
* [**SerializationException when serializing lots of objects in .NET**](http://stackoverflow.com/questions/569127/serializationexception-when-serializing-lots-of-objects-in-net)
* [**Large objects serialization with C#**](http://engineering.picscout.com/2013/05/large-objects-serialization-with-c.html)