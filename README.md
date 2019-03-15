# Veruthian.Library (v0.1.0)
A shared group of libraries for all Veruthian dotnet projects.

**Veruthian.Library.Collections**
-- 
- Contains a new hiearchy of collection interfaces/classes based on IEnumerable
- Extension methods to easily created collections from existing types

##### Containers
- IContainer<T> : IEnumerable<T>
- IExpandableContainer<T> : IContainer<T> => DataSet
- IPool<K, A> : IContainer<(K, A)>
        
##### Lookups
- ILookup<K, V> : IContainer<V>  => NestedDataLookup, SequentialDataLookup
- IMutableLookup<K, V> : ILookup<K, V>
- IExpandableLookup<K, V> : ILookup<K, V> => DataLookup

##### Indices
- IIndex<K, V>: ILookup<K, V>
- IMutableIndex<K, V>: IIndex<K, V>, IMutableLookup<K, V> => DataArray
- IOrderedIndex<K, V>: IIndex<K, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
- IExpandableIndex<K, V> : IIndex<K, V>, IExpandableLookup<K, V>, IExpandableContainer<V> => DataList

**Veruthian.Libray.Numeric**
-- 
- Contains interfaces for generalizing features of numerics
- Powerful BitTwiddler struct to manipulate 64 bits in a variety of way
- Contains Range and RangeSet classes for dealing with ranged values

**Veruthian.Library.Processing**
--
- Contains interfaces for basic data processing "machines"
- Extensions for streams and transformers

**Veruthian.Library.Readers**
--
- Contains generic classes for reading through IEnumerables
   - Reader -> Reads one item at a time
   - LookaheadReader -> capability of lookahead n/unlimited items
   - SpeculativeReader -> capable of looking ahead or marking and backtracking
   
- Extensions to easily create readers from IEnumerable

**Veruthian.Library.Text**
--
**Veruthian.Libary.Text.Encodings**
-- -
- Contains classes for processing Utf8, Utf16, and Utf32
- Contains Transformers for processing bytes into Utf32

**Veruthian.Libary.Text.Lines**
-- -
- Contains classes for processing text into lines

**Veruthian.Libary.Text.Runes**
-- -
- Contains classes for dealing with Runes/Codepoints
- Rune, RuneString, RuneBuffer,RuneLineTable,RuneSet

**Veruthian.Libary.Text.Chars**
-- -
- Contains classes for dealing with chars/strings

**Veruthian.Library.Types**
--
- Better Enum class base
- TypeSets and Extensions

**Veruthian.Library.Utility**
--
- Utility functionality 
   - IsNull extension
   - HashCode generator