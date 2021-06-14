using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HashTable5Tests")]
namespace HashTable5
{
    public class HashTable5<T> where T : class, IEquatable<T>, IIdable
    {
        private const int BLFL = 18;
        internal List<LinkedList<Tuple<int, T>>> table { get; set; }
        internal int BucketLoadFactorLimit { get; private set; } = BLFL;
        internal float CurrentBucketsLF => (float)TotalRecords / (float)LoadedBucketCount;
        internal float CurrentTableLF => (float)LoadedBucketCount / (float)table.Count;
        internal int LoadedBucketCount { get; set; }
        internal float LoadFactorLimit { get; private set; }
        internal int TotalRecords { get; set; }

        /// <summary>
        /// Initializes a new Link List-chained Hash Table with an initial capacity of 16.
        /// </summary>
        public HashTable5()
        {
            table = new List<LinkedList<Tuple<int, T>>>(16);
            InitializeBuckets();
            LoadFactorLimit = 0.51f;
        }

        /// <summary>
        /// Initializes a new Link List-chained Hash Table with an initial capacity of 16 and a user-defined maximum load factor.
        /// maxLoadFactor must be between 0 and 1 e.g. 0.51
        /// </summary>
        /// <param name="maxLoadFactor"></param>
        public HashTable5(float maxLoadFactor) : this()
        {
            LoadFactorLimit = maxLoadFactor;
        }

        /// <summary>
        /// Initializes a new Link List-chained Hash Table with a custom initial capacity and user-defined maximum load factor.
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="maxLoadFactor"></param>
        public HashTable5(int capacity, float maxLoadFactor)
        {
            table = new List<LinkedList<Tuple<int, T>>>(capacity);
            InitializeBuckets();
            LoadFactorLimit = maxLoadFactor;
        }

        /// <summary>
        /// Private function creates an index to a record based on object.Id.
        /// Used as primary key for storing and finding records.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int HashFunction(int id) => id % table.Capacity;

        /// <summary>
        /// Private method initializes LinkedLists in each index of the internal list.
        /// </summary>
        private void InitializeBuckets()
        {
            for (int idx = 0; idx < table.Capacity; idx++)
            {
                table.Add(new LinkedList<Tuple<int, T>>());
            }
            LoadedBucketCount = 0;
            TotalRecords = 0;
        }

        /// <summary>
        /// Add a new Person object to the hashtable. Will not load a duplicate ID, nor an ID of less than 1.
        /// Caller is responsible for disallowing duplicate object loading.
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        public bool AddEntry(T newEntry)
        {
            if (newEntry.Id < 1)
            {
                return false;
            }
            var hash = HashFunction(newEntry.Id);
            var newItem = new Tuple<int, T>(hash, newEntry);
            LinkedList<Tuple<int, T>> bucket = null;

            try
            {
                bucket = this.table[hash];
                if (bucket.Count < 1)
                {
                    LoadedBucketCount++;
                }
                bucket.AddLast(new LinkedListNode<Tuple<int, T>>(newItem));
                TotalRecords++;
                var checkLF = CheckLF();
                //  TODO: log the fact that checkLF() was run and the checkLF integer result
            }
            catch (InvalidOperationException ioe)
            {
                //  TODO: log this possible specific exception event
                var errMsg = $"{ ioe.Message }; Entry: { newEntry }";
            }
            catch (Exception ex)
            {
                //  TODO: log this unexpected event
                var errMsg = $"{ ex.Message }; Entry: { newEntry }";
            }

            return true;
        }

        /// <summary>
        /// Returns a concrete Person instance by using a Person.Id field.
        /// Returns null if nothing found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            var bucket = new LinkedListNode<Tuple<int, T>>(null);
            
            if (Find(id, ref bucket))
            {
                return bucket.Value.Item2;
            }

            return (T)(default);
        }

        /// <summary>
        /// Changes the Person.Id to -1 so a scavenger process can remove the object during a consolidation or rehash operation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Remove(int id)
        {
            var bucket = new LinkedListNode<Tuple<int, T>>(null);

            if (Find(id, ref bucket))
            {
                bucket.Value.Item2.Id = -1;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Private method used by Get() and Remove() returns boolean result and a reference to an object edited by this code block.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bucket_node"></param>
        /// <returns></returns>
        private bool Find(int id, ref LinkedListNode<Tuple<int, T>> bucket_node)
        {
            int index = HashFunction(id);
            var bucket = table[index];
            bucket_node = bucket.First;
            var nodeIdx = 0;
            while (nodeIdx < bucket.Count)
            {
                if (bucket_node.Value.Item2.Id == id)
                {
                    return true;
                }
                bucket_node = bucket_node.Next;
                nodeIdx++;
            }
            //  TODO: log this situation including nodeIdx and bucket.Count for debugging

            bucket = null;
            return false;
        }

        /// <summary>
        /// Removes all elements and chained indices from this hashtable. Other hashtable configuration remains unchanged.
        /// </summary>
        public void Clear()
        {
            for(int i=0; i < table.Capacity; i++)
            {
                table[i].Clear();
            }
            LoadedBucketCount = 0;
            TotalRecords = 0;
        }

        /// <summary>
        /// Private method calculates live Load Factor and triggers a rehash if it is above threshold.
        /// </summary>
        /// <returns></returns>
        private int CheckLF()
        {
            //  tests load factor to see if it is time to rehash
            //  call this method during AddPerson when a new LinkedList is added (not new LL Node)
            if (CurrentTableLF > LoadFactorLimit)
            {
                //  TODO: log a warning message with the actual LoadFactor

                if (CurrentBucketsLF > BucketLoadFactorLimit)
                {
                    //  TODO: log a warning message that buckets are full and a rehash is happening now
                    if (DoReHash() < 0)
                    {
                        //  TODO: log an error message that a rehash failed

                    }
                }
            }

            else if (CurrentTableLF <= LoadFactorLimit)
            {
                //  TODO: log an informational message with actual LoadFactor
            }

            return 0;
        }

        /// <summary>
        /// Expand the hashtable and rehash (reindex) all records. Returns 0 upon success, -1 if there are errors.
        /// This is an expensive process but should be performed preemptively to keep read operation performance high.
        /// </summary>
        /// <param name="hashTable"></param>
        /// <returns></returns>
        internal int DoReHash()
        {
            int result = -1;
            
            //  1.  create a new List of type <Person>with a capacity of at least the number of initialized buckets
            List<T> NewList = new List<T>(TotalRecords);

            //  2.  cycle through all LinkedLists with count > 0
            if (MoveTo(NewList, 0) < TotalRecords)
            {
                //  TODO: log this event

                //  avoid running any more code since the copy failed
                NewList = null; //  clean-up memory since CopyTo failed
                return result;
            }

            //  4.  move valid data into a new hashtable with LL chaining using a new table size that reduces LF without over-provisioning capacity
            int recordsBucketsSum = TotalRecords + table.Capacity;
            int newBucketCalc = (int)(recordsBucketsSum * 0.8f);
            int newBucketCount = Math.Max(TotalRecords, newBucketCalc);

            if (newBucketCount % 2 == 0)
            {
                newBucketCount++;    //  force an odd number outcome (prime would be better but not necessary here)
            }

            try
            {
                table = new List<LinkedList<Tuple<int, T>>>(newBucketCount);
                InitializeBuckets();
                foreach (var item in NewList)
                {
                    if (this.AddEntry(item))
                    {
                        //  5.  update LoadedBuckets property upon successful data load events (happens in AddPerson method)
                    }
                    else
                    {
                        //  a problem occurred adding a record but was not an exception
                        //  TODO: log this warning event but keep going
                    }
                }
                result = 0;
            }
            catch (Exception ex)
            {
                //  6.  catch any errors and roll-back before making permanent changes
                //  TODO: roll-back table to previous state

                //  7.  log all operations
                result = -1;
            }
            //  8.  return 0 with success, -1 with failure
            return result;
        }

        /// <summary>
        /// Moves the hashtable elements to a one-dimensional array instance starting at the specified index.
        /// This method is based on .NET5 System.Collections.Hashtable.CopyTo with additional function that
        /// filters source items before moving to destination array and removes copied items from source bucket LinkedList.
        /// </summary>
        /// <param name="resultingList"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public int MoveTo(List<T> resultingList, int startIndex)
        {
            var itemsAffected = 0;

            for (int idx = startIndex; idx < table.Count; idx++)
            {
                var node = table[idx];
                while (node.Count > 0)
                {
                    var data = node.First.Value.Item2;
                    //  3.  filter-out any Person.Id with value less than 1 and any duplicate where Person.Equals(OtherPerson) and Person.Id is same
                    if (data != null && data.Id > 0)
                    {
                        resultingList.Add(data);
                        itemsAffected++;
                    }
                    node.RemoveFirst();
                }
            }

            return itemsAffected;
        }

    }
}
