# SQL for Unity
A series of APIs that allows anyone, even with no knowledge of SQL syntax, to access a SQLite Database from Unity.

These scripts have been tested and confirmed to work on **Windows**, **Unity Editor** and **Meta Quest 2**

## APIs
- **`LoadDatabase`:** Load a database located in `PersistentDataPath` when provided with **filename**. 
    - This will create a new database if it didn't exist.
- **`Verify`:** Use this function to check if the database exists or not.
- **`CreateTable`:** Create a new table with **name** and specified **columns** and **datatypes**.
- **`InsertData`:** Insert an entry to a **table**, with specified **value** per **column**.
- **`Query`:** Retrive an array of `strings` containing the data in specified **columns** of specified **table**.
- **`ModifyData`:** Modify an entry in a **table** according to specified **condition**, with new **value** for the **column**.
- and more...

## Reference
- https://github.com/walidabazo/SQLiteUnity3d_Android
- https://github.com/robertohuertasm/SQLite4Unity3d