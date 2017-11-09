export default _formatDateForBackend = (date) => {
  return date.toISOString().slice(0, 19).replace(/T/g, " ");
};
