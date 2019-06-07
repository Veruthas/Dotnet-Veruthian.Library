# Veruthian.Library (v0.1.0)
A shared group of libraries for all Veruthian dotnet projects.

**Veruthian.Library.Collections**
-- 
- Contains a new hiearchy of collection interfaces/classes based on IEnumerable
- Extension methods to easily created collections from existing types

##### Containers
- IContainer<T> : IEnumerable<T>
- IExpandableContainer<T> : IContainer<T> => DataSet
- IPool<A, D> : IContainer<(K, A)>
        
##### Lookups
- ILookup<A, V> : IContainer<V>  => NestedDataLookup, SequentialDataLookup
- IMutableLookup<A, V> : ILookup<A, V>
- IExpandableLookup<A, V> : ILookup<A, V> => DataLookup

##### Vectors
- IVector<A, V>: ILookup<A, V>
- IMutableVector<A, V>: IVector<A, V>, IMutableLookup<A, V> => DataArray
- IOrderedVector<A, V>: IVector<A, V>, IExpandableContainer<V> => SortedDataList, SortedDataSet
- IExpandableVector<A, V> : IVector<A, V>, IExpandableLookup<A, V>, IExpandableContainer<V> => DataList

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