namespace HSA.RehaGame.DB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Models;
    using Mono.Data.Sqlite;

    public interface IDatabase
    {
        SQLiteErrorCode Save(Type model, List<PropertyInfo> fields, List<object> values);

        SQLiteErrorCode AddManyToManyRelation(ManyToManyRelation attribute, object sourceId, IDictionary models);

        SQLiteErrorCode Delete(Type model, string primaryKeyName, object primaryKeyValue);

        SQLiteErrorCode UpdateTable(TableColumn attribute, Type model, PropertyInfo column, object value, string primaryKeyName, object primaryKeyValue);

        object[] All(string primaryKeyField, string modelName);

        object[] Get(TableColumn attribute, PropertyInfo column, Type model, string primaryKeyName, object primaryKeyValue);

        object[] Join(ManyToManyRelation attribute, object primaryKeyValue);
    }
}
