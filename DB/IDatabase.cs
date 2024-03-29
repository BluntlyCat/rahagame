﻿namespace HSA.RehaGame.DB
{
    using Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IDatabase
    {
        IModel GetCachedModel(Type modelType, object primaryKeyValue);

        void SetCachedModel(Type modelType, object primaryKeyValue, IModel model);

        TransactionResult Save(string primaryKeyName, Type model, List<PropertyInfo> fields, List<object> values);

        TransactionResult AddManyToManyRelations(ManyToManyRelation attribute, object sourceId, IDictionary models);

        TransactionResult AddManyToManyRelation(ManyToManyRelation attribute, object sourceId, Model model);

        TransactionResult Delete(Type model, string primaryKeyName, object primaryKeyValue);

        TransactionResult UpdateTable(TableColumn attribute, Type model, PropertyInfo column, object value, string primaryKeyName, object primaryKeyValue);

        object[] All(string primaryKeyField, string modelName);

        object[] Get(TableColumn attribute, PropertyInfo column, Type model, PrimaryKey pkAttribute, PropertyInfo primaryKey, object primaryKeyValue);

        object[] Join(ManyToManyRelation attribute, object primaryKeyValue);
    }
}
