﻿using System;
using GoLive.Saturn.Data.Entities;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GoLive.Saturn.Data.EntitySerializers
{
    public class TimestampSerializer : ClassSerializerBase<Timestamp>
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override Timestamp Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            var ts = new Timestamp();

            bsonReader.ReadStartDocument();

            var CreatedDate = bsonReader.ReadDateTime("CreatedDate");
            var LastModifiedDate = bsonReader.ReadDateTime("LastModifiedDate");

            bsonReader.ReadEndDocument();

            ts.CreatedDate = epoch.AddMilliseconds(CreatedDate);
            ts.LastModifiedDate = epoch.AddMilliseconds(LastModifiedDate);

            return ts;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Timestamp value)
        {
            if (!value.CreatedDate.HasValue)
            {
                value.CreatedDate = DateTime.UtcNow;
            }

            value.LastModifiedDate = DateTime.UtcNow;

            context.Writer.WriteStartDocument();

            context.Writer.WriteName("CreatedDate");
            context.Writer.WriteDateTime(getEpoch(value.CreatedDate.Value));


            context.Writer.WriteName("LastModifiedDate");
            context.Writer.WriteDateTime(getEpoch(value.LastModifiedDate.Value));


            context.Writer.WriteEndDocument();
        }

        long getEpoch(DateTime dateTime)
        {

            return (long)dateTime.ToUniversalTime().Subtract(epoch).TotalMilliseconds;
        }
    }
}