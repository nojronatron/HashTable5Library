using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Q90_HashTables.Tests
{
    [TestClass()]
    public class HashTable5Tests
    {
        [TestMethod()]
        public void DefaultCTOR_Test()
        {
            var expectedResult = "HashTable5";
            var expectedLFSet = true;

            var ht5 = new HashTable5();
            var actualResult = ht5.GetType().Name;
            var actualLFSet = (ht5.LoadFactorLimit > 0 && ht5.LoadFactorLimit < 1);

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("Expected TableLoadFactor will be between 0 and 1. Dummy entry", 0.0f),
                new Tuple<string, float>("Actual TableLoadFactor setting", ht5.LoadFactorLimit)
            }
            );
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedLFSet, actualLFSet);
        }

        [TestMethod()]
        public void SingleArgCTOR_Test()
        {
            var expectedResult = "HashTable5";
            var expectedLF = 0.8f;

            var ht5 = new HashTable5(expectedLF);
            var actualResult = ht5.GetType().Name;
            var actualLFSetting = ht5.LoadFactorLimit;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("Expected TableLoadFactor will be between 0 and 1. Dummy entry", 0.0f),
                new Tuple<string, float>("Actual TableLoadFactor setting", actualLFSetting)
            }
            );

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedLF, actualLFSetting);
        }

        [TestMethod()]
        public void TwoArgCTOR_Test()
        {
            var expectedResult = "HashTable5";
            var expectedLF = 0.8f;
            var expectedCapacity = 5;

            var ht5 = new HashTable5(expectedCapacity, expectedLF);
            var actualResult = ht5.GetType().Name;
            var actualLFSetting = ht5.LoadFactorLimit;
            var actualCapacity = ht5.table.Capacity;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("Expected TableLoadFactor will be between 0 and 1. Dummy entry", 0.0f),
                new Tuple<string, float>("Actual TableLoadFactor setting", actualLFSetting),
                new Tuple<string, float>("actual capacity", actualCapacity)
            }
            );

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedLF, actualLFSetting);
            Assert.AreEqual(expectedCapacity, actualCapacity);
        }

        [TestMethod()]
        public void AddPerson_Test()
        {
            var expectedAddPersonResult = true;
            var expectedLoadedBucketCount = 1;
            var expectedTotalRecords = 1;

            var ht5 = new HashTable5();
            var actualAddPersonResult = ht5.AddPerson(new Person(1, "fn", "ln", "e@mail.ext"));
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("Actual loaded bucket count", actualLoadedBucketCount),
                new Tuple<string, float>("Actual total records", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
        }

        [TestMethod()]
        public void AddLotsOfPersons_Test()
        {
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;

            var ht5 = new HashTable5(20, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
        }

        [TestMethod()]
        public void GetById_Test()
        {
            var idToFind = 5;
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;
            var PersonToFind = new Person(5, "Ean", "Echo", "Ean@Echo.net");

            var ht5 = new HashTable5(20, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;

            var actualPersonFound = ht5.GetById(idToFind);
            var correctPersonReturned = actualPersonFound.Equals(PersonToFind);
            var personHasCorrectId = actualPersonFound.Id == idToFind;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(correctPersonReturned);
            Assert.IsTrue(personHasCorrectId);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var idToFind = 5;
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;

            var ht5 = new HashTable5(20, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;

            var actualRemoveRecordResult = ht5.Remove(idToFind);

            var actualPersonFound = ht5.GetById(idToFind);
            var personCannotBeFound = actualPersonFound == null;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(actualRemoveRecordResult);
            Assert.IsTrue(personCannotBeFound);
        }

        [TestMethod()]
        public void ClearTest()
        {
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;

            var expectedClearedBucketCount = 0;
            var expectedCLearedRecordsCount = 0;

            var ht5 = new HashTable5(20, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;
            var lfAfterPeopleLoaded = ht5.CurrentTableLF;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords),
                new Tuple<string, float>("LFAfterPeopleLoaded", lfAfterPeopleLoaded)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(lfAfterPeopleLoaded > 0);

            ht5.Clear();

            var lfAfterCleared = ht5.CurrentTableLF;
            actualLoadedBucketCount = ht5.LoadedBucketCount;
            actualTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords),
                new Tuple<string, float>("LFAfterCleared", lfAfterCleared)
            }
            );

            Assert.AreEqual(expectedClearedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedCLearedRecordsCount, actualTotalRecords);
            Assert.IsTrue(lfAfterCleared == 0);
        }

        [TestMethod()]
        public void MoveTo_Test()
        {
            var ht5 = new HashTable5(8, 1);
            var addPeopleResult = LoadInitialPeople(ht5);

            var expectedEndCount = 4;

            var existingItems = new List<Person>();
            for(int idx=4; idx < ht5.LoadedBucketCount; idx++)
            {
                existingItems.Add(ht5.table[idx].First.Value.Item2);
            }

            var newList = new List<Person>();
            ht5.MoveTo(newList, 4);

            var actualEndCount = newList.Count;

            var existingItemsIdx = 0;
            var ht5Idx = 4;
            foreach(var item in newList)
            {
                var originalItem = existingItems[existingItemsIdx];
                Assert.IsTrue(item.Equals(originalItem));
                Assert.IsTrue(ht5.table[ht5Idx].Count == 0);
                existingItemsIdx++;
                ht5Idx++;
            }

            Assert.AreEqual(expectedEndCount, actualEndCount);
        }

        [TestMethod()]
        public void ReHash_Test()
        {
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;

            var ht5 = new HashTable5(8, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);

            expectedAddPersonResult = 16;
            expectedLoadedBucketCount = 8;
            expectedTotalRecords = 16;

            actualAddPersonResult = LoadMorePeople(actualAddPersonResult, ht5);
            actualLoadedBucketCount = ht5.LoadedBucketCount;
            actualTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);

            var expectedDoReHashResult = 0;
            var preReHashLoadedBucketCount = ht5.LoadedBucketCount;
            var preReHashAvgBucketsLF = ht5.CurrentBucketsLF;
            var preReHashLoadFactor = ht5.CurrentTableLF;

            var actualDoReHashResult = ht5.DoReHash();

            actualTotalRecords = ht5.TotalRecords;
            actualLoadedBucketCount = ht5.LoadedBucketCount;
            var postReHashAvgBucketsLF = ht5.CurrentBucketsLF;
            var postReHashLoadFactor = ht5.CurrentTableLF;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("preReHashLoadedBucketCount", preReHashLoadedBucketCount),
                new Tuple<string, float>("preReHashAvgBucketsLF", preReHashAvgBucketsLF),
                new Tuple<string, float>("preReHashLoadFactor", preReHashLoadFactor),
                new Tuple<string, float>("actualDoReHashResult", actualDoReHashResult),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("postReHashAvgBucketsLF", postReHashAvgBucketsLF),
                new Tuple<string, float>("postReHashLoadFactor", postReHashLoadFactor)
            }
            );

            Assert.AreEqual(expectedDoReHashResult, actualDoReHashResult);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(actualLoadedBucketCount > preReHashLoadedBucketCount);
            Assert.IsTrue(preReHashAvgBucketsLF > postReHashAvgBucketsLF);
            Assert.IsTrue(preReHashLoadFactor > postReHashLoadFactor);
        }

        [TestMethod()]
        public void AddEvenMorePersons_Test()
        {
            var expectedAddPersonResult = 8;
            var expectedLoadedBucketCount = 8;
            var expectedTotalRecords = 8;
            var expectedAvgBucketsLF = 0.0f;

            var ht5 = new HashTable5(10, 0.8f);
            var actualAddPersonResult = LoadInitialPeople(ht5);
            var actualLoadedBucketCount = ht5.LoadedBucketCount;
            var actualTotalRecords = ht5.TotalRecords;
            var actualAvgBucketsLF = ht5.CurrentBucketsLF;
            var actualLoadFactor = ht5.CurrentTableLF;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("actualAddPersonResult", actualAddPersonResult),
                new Tuple<string, float>("actualLoadedBucketCount", actualLoadedBucketCount),
                new Tuple<string, float>("actualTotalRecords", actualTotalRecords),
                new Tuple<string, float>("actualAvgBucketsLF", actualAvgBucketsLF),
                new Tuple<string, float>("actualLoadFactor", actualLoadFactor)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, actualAddPersonResult);
            Assert.AreEqual(expectedLoadedBucketCount, actualLoadedBucketCount);
            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(actualAvgBucketsLF > expectedAvgBucketsLF);

            expectedAddPersonResult = 16;
            expectedTotalRecords = 16;

            var postLMPAddPersonResult = LoadMorePeople(actualAddPersonResult, ht5);

            var postLMPLoadedBucketCount = ht5.LoadedBucketCount;
            var postLMPAvgBucketsLF = ht5.CurrentBucketsLF;
            var postLMPLoadFactor = ht5.CurrentTableLF;
            var postLMPTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("postLMPAddPersonResult", postLMPAddPersonResult),
                new Tuple<string, float>("postLMPLoadedBucketCount", postLMPLoadedBucketCount),
                new Tuple<string, float>("postLMPAvgBucketsLF", postLMPAvgBucketsLF),
                new Tuple<string, float>("postLMPLoadFactor", postLMPLoadFactor),
                new Tuple<string, float>("postLMPTotalRecords", postLMPTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, postLMPAddPersonResult);
            Assert.AreEqual(expectedTotalRecords, postLMPTotalRecords);
            Assert.IsTrue(expectedLoadedBucketCount < postLMPLoadedBucketCount);
            Assert.IsTrue(actualLoadFactor < postLMPLoadFactor);
            Assert.IsTrue(postLMPAvgBucketsLF > actualAvgBucketsLF);

            expectedAddPersonResult = 26;
            expectedTotalRecords = 26;

            var postLEMPAddPersonResult = LoadEvenMorePeople(postLMPAddPersonResult, ht5);

            var postLEMPLoadedBucketCount = ht5.LoadedBucketCount;
            var postLEMPAvgBucketsLF = ht5.CurrentBucketsLF;
            var postLEMPLoadFactor = ht5.CurrentTableLF;
            var postLEMPTotalRecords = ht5.TotalRecords;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("postLEMPAddPersonResult", postLEMPAddPersonResult),
                new Tuple<string, float>("postLEMPLoadedBucketCount", postLEMPLoadedBucketCount),
                new Tuple<string, float>("postLEMPAvgBucketsLF", postLEMPAvgBucketsLF),
                new Tuple<string, float>("postLEMPLoadFactor", postLEMPLoadFactor),
                new Tuple<string, float>("postLEMPTotalRecords", postLEMPTotalRecords)
            }
            );

            Assert.AreEqual(expectedAddPersonResult, postLEMPAddPersonResult);
            Assert.AreEqual(expectedTotalRecords, postLEMPTotalRecords);
            Assert.AreEqual(postLMPLoadedBucketCount, postLEMPLoadedBucketCount);
            Assert.IsTrue(postLMPAvgBucketsLF < postLEMPAvgBucketsLF);
            Assert.AreEqual(postLMPLoadFactor, postLEMPLoadFactor);
        }

        [TestMethod()]
        public void TriggerBucketLFLimitReHash_Test()
        {
            //  1.  find a LARGE text file of People objects that can be loaded into this test project in chunks
            //  check out mockaroo
            //  2.  set the initial table size to 5 and LF to 80%
            var ht5 = new HashTable5(5, 0.8f);

            //  3.  load about 80 records (bucketLFLimit default is 18)
            var fileName = "onehundredpeople.json";
            var currDir = Directory.GetCurrentDirectory();
            var targetFile = Path.Combine(currDir, fileName);
            List<JsonTestPerson> peopleList = JsonConvert.DeserializeObject<List<JsonTestPerson>>(File.ReadAllText(targetFile));

            var tableLFLimit = ht5.LoadFactorLimit;
            var bucketLFLimit = ht5.BucketLoadFactorLimit;
            var preBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            var preBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            var preBulkLoad_CurrentTableLF = ht5.CurrentTableLF;
            var expectedTotalRecords = 85;

            BulkLoadPeople(peopleList, ht5, 0, expectedTotalRecords);

            //  4.  run asserts and digitaldisplayresults() and check for CurrentTableLF > LoadFactorLimit && BucketsLoadFactor > BucketLFLimit
            var actualTotalRecords = ht5.TotalRecords;
            var postBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            var postBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            var postBulkLoad_CurrentTableLF = ht5.CurrentTableLF;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("First BulkLoadPeople call:", expectedTotalRecords),
                new Tuple<string, float>("BucketLoadFactorLimit", ht5.BucketLoadFactorLimit),
                new Tuple<string, float>("CurrentBuckesLF", ht5.CurrentBucketsLF),
                new Tuple<string, float>("CurrentTableLF", ht5.CurrentTableLF),
                new Tuple<string, float>("LoadedBucketCount", ht5.LoadedBucketCount),
                new Tuple<string, float>("LoadFactorLimit", ht5.LoadFactorLimit),
                new Tuple<string, float>("TotalRecords", ht5.TotalRecords)
            }
            );

            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(preBulkLoad_CurrentTableLF < postBulkLoad_CurrentTableLF);
            Assert.IsTrue(preBulkLoad_LoadedBucketCount < postBulkLoad_LoadedBucketCount);
            Assert.IsTrue(preBulkLoad_CurrentBucketsLF != postBulkLoad_CurrentBucketsLF);

            //  5.  add 5 more records
            tableLFLimit = ht5.LoadFactorLimit;
            bucketLFLimit = ht5.BucketLoadFactorLimit;
            preBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            preBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            preBulkLoad_CurrentTableLF = ht5.CurrentTableLF;
            expectedTotalRecords = 90;

            BulkLoadPeople(peopleList, ht5, 85, expectedTotalRecords);

            actualTotalRecords = ht5.TotalRecords;
            postBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            postBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            postBulkLoad_CurrentTableLF = ht5.CurrentTableLF;

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("First BulkLoadPeople call:", expectedTotalRecords),
                new Tuple<string, float>("BucketLoadFactorLimit", ht5.BucketLoadFactorLimit),
                new Tuple<string, float>("CurrentBuckesLF", ht5.CurrentBucketsLF),
                new Tuple<string, float>("CurrentTableLF", ht5.CurrentTableLF),
                new Tuple<string, float>("LoadedBucketCount", ht5.LoadedBucketCount),
                new Tuple<string, float>("LoadFactorLimit", ht5.LoadFactorLimit),
                new Tuple<string, float>("TotalRecords", ht5.TotalRecords)
            }
            );

            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(preBulkLoad_CurrentTableLF <= postBulkLoad_CurrentTableLF);
            Assert.IsTrue(preBulkLoad_LoadedBucketCount == postBulkLoad_LoadedBucketCount);
            Assert.IsTrue(preBulkLoad_CurrentBucketsLF <= postBulkLoad_CurrentBucketsLF);

            //  6.  add more records
            tableLFLimit = ht5.LoadFactorLimit;
            bucketLFLimit = ht5.BucketLoadFactorLimit;
            preBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            preBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            preBulkLoad_CurrentTableLF = ht5.CurrentTableLF;
            expectedTotalRecords = 100;

            BulkLoadPeople(peopleList, ht5, 90, expectedTotalRecords+1);

            actualTotalRecords = ht5.TotalRecords;
            postBulkLoad_LoadedBucketCount = ht5.LoadedBucketCount;
            postBulkLoad_CurrentBucketsLF = ht5.CurrentBucketsLF;
            postBulkLoad_CurrentTableLF = ht5.CurrentTableLF;

            //  7.  output all properties

            DisplayDigitalResults(new List<Tuple<string, float>>
            {
                new Tuple<string, float>("First BulkLoadPeople call:", expectedTotalRecords),
                new Tuple<string, float>("BucketLoadFactorLimit", ht5.BucketLoadFactorLimit),
                new Tuple<string, float>("CurrentBuckesLF", ht5.CurrentBucketsLF),
                new Tuple<string, float>("CurrentTableLF", ht5.CurrentTableLF),
                new Tuple<string, float>("LoadedBucketCount", ht5.LoadedBucketCount),
                new Tuple<string, float>("LoadFactorLimit", ht5.LoadFactorLimit),
                new Tuple<string, float>("TotalRecords", ht5.TotalRecords)
            }
            );

            Assert.AreEqual(expectedTotalRecords, actualTotalRecords);
            Assert.IsTrue(preBulkLoad_CurrentTableLF == postBulkLoad_CurrentTableLF);
            Assert.IsTrue(preBulkLoad_LoadedBucketCount < postBulkLoad_LoadedBucketCount);
            Assert.IsTrue(preBulkLoad_CurrentBucketsLF >= postBulkLoad_CurrentBucketsLF);

        }

        private static bool BulkLoadPeople<T>(List<T> people, HashTable5 hashTable, int startIndex, int endIndex)
        {
            startIndex = startIndex < 0 ? 0 : startIndex;
            startIndex = startIndex >= people.Count ? people.Count - 1 : startIndex;
            endIndex = endIndex < 1 ? 1 : endIndex;
            endIndex = endIndex >= people.Count ? people.Count : endIndex;

            JsonTestPerson jtp = new JsonTestPerson();

            for (int idx = startIndex; idx < endIndex; idx++)
            {
                jtp = people[idx] as JsonTestPerson;
                Person p = new Person
                {
                    Id = jtp.Id,
                    FirstName = jtp.FirstName,
                    LastName = jtp.LastName,
                    Email = jtp.Email
                };
                hashTable.AddPerson(p);
            }
            return true;
        }

        private static int LoadInitialPeople(HashTable5 hashtable)
        {
            hashtable.AddPerson(new Person(1, "Abe", "Alpha", "Abe@Alpha.com"));
            hashtable.AddPerson(new Person(2, "Barb", "Bravo", "Barb@Bravo.net"));
            hashtable.AddPerson(new Person(3, "Chuck", "Charlie", "Chuck@Charlie.org"));
            hashtable.AddPerson(new Person(4, "Dan", "Delta", "Dano@Delta.com"));
            hashtable.AddPerson(new Person(5, "Ean", "Echo", "Ean@Echo.net"));
            hashtable.AddPerson(new Person(6, "Frank", "Foxtrot", "Frank@Foxtrot.org"));
            hashtable.AddPerson(new Person(7, "George", "Golf", "George@golfers.net"));
            hashtable.AddPerson(new Person(8, "Henry", "Hotel", "Henry@Hotel.com"));
            return 8;
        }

        private static int LoadMorePeople(int pId, HashTable5 hashtable)
        {
            if (pId < 0)
            {
                pId = 0;
            }

            pId++;
            hashtable.AddPerson(new Person(pId, "Indeo", "India", "Indeo@India.in"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Joey", "Juliet", "Joey@Juliette.net"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Kelli", "Kilo", "Kelli@Kilo.com"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Larry", "Lima", "Larry@Lima.org"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Mikey", "Mike", "MikeyMike@email.net"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Noah", "November", "November@Noah.net"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Ollie", "Oscar", "Ollie@Grouch.com"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Pete", "Papa", "PapaPete@Email.org"));
            return pId;
        }

        private static int LoadEvenMorePeople(int pId, HashTable5 hashtable)
        {
            if (pId < 0)
            {
                pId = 0;
            }

            pId++;
            hashtable.AddPerson(new Person(pId, "Quixote", "Quebec", "Quixote@Quebec.co"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Roger", "Roger", "Rog@RogerRoger.net"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Wilson", "Sierra", "Sierra.Wilson@Seahawks.com"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Thom", "Tango", "Tango.Tom@Dancers.info"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Udo", "Union", "Udo@Union.dk"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Victor", "Victoria", "VV@Victoria.co.uk"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Willy", "Whisky", "Willy@Wisky.org"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Xavier", "Xray", "Xavier@xray.org"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Yanni", "Yankee", "Yanni@Yank.us"));
            pId++;
            hashtable.AddPerson(new Person(pId, "Zanzi", "Zulu", "Zanzi.Zulu@ZeroHour.net"));
            return pId;
        }

        private static void DisplayDigitalResults(List<Tuple<string, float>> messages)
        {
            var sb = new StringBuilder();
            foreach (var item in messages)
            {
                sb.AppendLine($"{ item.Item1 }: { item.Item2}");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
