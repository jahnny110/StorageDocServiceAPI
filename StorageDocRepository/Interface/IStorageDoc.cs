namespace StorageDocRepository.Interfaces
{
    public interface IStorageDoc
    {
        /// <summary>
        /// To save a document to storage
        /// </summary>
        /// <param name="doc">Document</param>
        /// <returns>True - save succes, Flase - save failed</returns>
        Task<bool> SaveDocAsync(StorageDocument doc);

        /// <summary>
        /// To update a document in storage
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <param name="doc">Document</param>
        /// <returns>True - update succes, False - update failed</returns>
        Task<bool> UpdateDocAsync(string id, StorageDocument doc);

        /// <summary>
        /// To get a document from storage
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <returns>Returns document base on his id. Returns null if the document does not exist.</returns>
        Task<StorageDocument?> GetDocAsync(string id);

        /// <summary>
        /// To delete a document from storage
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <returns>True - deleted, False - delete failed</returns>
        Task<bool> DeleteDocAsync(string id);

    }
}
