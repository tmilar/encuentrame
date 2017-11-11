/**
 * Auxiliar function to have a delay in ms, compatible with async/await.
 *
 * @param time
 * @returns {Promise}
 * @private
 */
export default sleep = (time) => {
  return new Promise((resolve) => setTimeout(resolve, time));
};
