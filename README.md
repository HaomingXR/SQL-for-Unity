# SQL-for-Unity-Android
- A series of APIs that access SQLite Database from Unity, on both PC and Android Platforms
- Tested on Meta Quest 2 & Unity Editor

## APIs
Currently provide a few simple APIs to call easily without having to know SQL commands:
- `LoadDatabase`: Load a database located in PersistentDataPath when provided with filename. This will create a new database if it doesn't exist.
- `Verify`: Use this function to basically check if the database existed.
- `CreateTable`: Create a new table with **name**, with specified **columns** and **datatypes**
- `InsertData`: Insert an entry to a **table**, with specified **value** per **column**
- `ModifyData`: Modify an entry in a **table**, according to specified **id** and new **value** for the **column**
- `Query`: Retrive a string containing the data in specified **columns** of specified **table**
- `QueryAllWithFilter`: Retrive a string containing the data in a specific row of a **table**, according to **condition**

If the above commands are~~ trash ~~not adequate, you can also:
- `RunCustomCommand`: Run a command you specified

### Reference
- https://github.com/walidabazo/SQLiteUnity3d_Android
- https://github.com/robertohuertasm/SQLite4Unity3d
