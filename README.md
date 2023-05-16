# SQL for Unity Android
- A series of APIs that access SQLite Database from Unity, on both PC and Android platforms
  - Tested on **Windows**, **Unity Editor** and **Meta Quest 2**

## APIs
Currently, provides a few simple APIs to call easily without having to know any SQL commands:
- **`LoadDatabase`:** Load a database located in `PersistentDataPath` when provided with **filename**. This will create a new database if it doesn't exist.
- **`Verify`:** Use this function to check if the database already existed.
- **`CreateTable`:** Create a new table called **name**, with specified **columns** and **datatypes**.
- **`InsertData`:** Insert an entry to a **table**, with specified **value** per **column**.
- **`Query`:** Retrive an array of `strings` containing the data in specified **columns** of specified **table**.
- **`ModifyData`:** Modify an entry in a **table** according to specified **condition**, with new **value** for the **column**.
- *And more...*

## Reference
- https://github.com/walidabazo/SQLiteUnity3d_Android
- https://github.com/robertohuertasm/SQLite4Unity3d
