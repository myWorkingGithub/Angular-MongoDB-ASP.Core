﻿﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  using AngularMongoASP.Data;
  using AngularMongoASP.Models;
using MongoDB.Driver;
 using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.GridFS;

 namespace AngularMongoASP.Services
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes();
        Task<Note> GetNote(string id);
        Task AddNote(Note item);
        Task<DeleteResult> RemoveNote(string id);

        Task<UpdateResult> UpdateNote(string id, string body);

        // demo interface - full document update
        Task<ReplaceOneResult> UpdateNoteDocument(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<DeleteResult> RemoveAllNotes();

        Task<ObjectId> UploadFile(IFormFile file);

        Task<String> GetFileInfo(string id);
    }
    public class NoteRepository : INoteRepository
    {
        private readonly DataContext _dataContext = null;

        public NoteRepository(IDatabaseSettings databaseSettings)
        {
            _dataContext = new DataContext(databaseSettings);
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            try
            {
                return await _dataContext.Notes.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Note> GetNote(string id)
        {
            var filter = Builders<Note>.Filter.Eq("Id", id);

            try
            {
                return await _dataContext.Notes
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddNote(Note item)
        {
            try
            {
                await _dataContext.Notes.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<DeleteResult> RemoveNote(string id)
        {
            try
            {
                return await _dataContext.Notes.DeleteOneAsync(
                     Builders<Note>.Filter.Eq("Id", id));
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<UpdateResult> UpdateNote(string id, string body)
        {
            var filter = Builders<Note>.Filter.Eq(s => s.Id, id);
            var update = Builders<Note>.Update
                            .Set(s => s.Body, body)
                            .CurrentDate(s => s.UpdatedOn);

            try
            {
                return await _dataContext.Notes.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ReplaceOneResult> UpdateNote(string id, Note item)
        {
            try
            {
                return await _dataContext.Notes
                            .ReplaceOneAsync(n => n.Id.Equals(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // Demo function - full document update
        public async Task<ReplaceOneResult> UpdateNoteDocument(string id, string body)
        {
            var item = await GetNote(id) ?? new Note();
            item.Body = body;
            item.UpdatedOn = DateTime.Now;

            return await UpdateNote(id, item);
        }

        public async Task<DeleteResult> RemoveAllNotes()
        {
            try
            {
                return await _dataContext.Notes.DeleteManyAsync(new BsonDocument());
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                return await _dataContext.Bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                return new ObjectId(ex.ToString());
            } 
        }

        public async Task<String> GetFileInfo(string id)
        {
            GridFSFileInfo info = null;
            var objectId = new ObjectId(id);
            try
            {
                using (var stream = await _dataContext.Bucket.OpenDownloadStreamAsync(objectId))
                {
                    info = stream.FileInfo;
                }
                return info.Filename;
            }
            catch (Exception)
            {
                return "Not Found";
            }
        }
    }
}
