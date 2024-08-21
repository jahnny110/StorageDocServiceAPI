using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageDocRepository.Repositories
{
    public interface IStorageDocRepository
    {
        /// <summary>
        /// To create new document entry in repository
        /// </summary>
        /// <param name="jsonDocStr">Document in Json string format</param>
        /// <returns>True - created, False - create failed</returns>
        Task<bool> CreateAsync(string jsonDocStr);

        /// <summary>
        /// Retrieve particular document entry from repository base on its id
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <returns>Returns document base on his id. Returns null if the document does not exist.</returns>
        Task<StorageDocument?> RetrieveAsync(string id);

        /// <summary>
        /// To update a document in repository
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <param name="jsonDocStr">Document in Json string format</param>
        /// <returns>True - updated, False - update failed</returns>
        Task<bool> UpdateAsync(string id, string jsonDocStr);

        /// <summary>
        /// To delete a document from repository
        /// </summary>
        /// <param name="id">Id of document</param>
        /// <returns>True - deleted, False - delete failed</returns>
        Task<bool> DeleteAsync(string id);
    }
}
