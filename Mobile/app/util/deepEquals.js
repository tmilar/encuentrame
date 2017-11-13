/**
 * Source:
 * https://stackoverflow.com/a/10316616/6279385
 *
 * note [1]: You may want to override the noted line with a custom notion of equality, which you'll also have to change in the other functions anywhere it appears. For example, do you or don't you want NaN==NaN? By default this is not the case. There are even more weird things like 0=='0'. See https://stackoverflow.com/a/5447170/711085 )
 *
 */

/**
 * compare deeply objects {a} and {b}
 * @param a
 * @param b
 * @returns {boolean}
 */
export const deepEquals = (a, b) => {
  if((a === null && b !== null) || (a !== null && b === null))
    return false;
  else if (a === null && b === null)
    return true;
  if (a instanceof Array && b instanceof Array)
    return arraysEqual(a, b);
  if (Object.getPrototypeOf(a) === Object.prototype && Object.getPrototypeOf(b) === Object.prototype)
    return objectsEqual(a, b);
  if (a instanceof Map && b instanceof Map)
    return mapsEqual(a, b);
  if (a instanceof Set && b instanceof Set)
    throw "Error: set equality by hashing not implemented.";
  if ((a instanceof ArrayBuffer || ArrayBuffer.isView(a)) && (b instanceof ArrayBuffer || ArrayBuffer.isView(b)))
    return typedArraysEqual(a, b);
  return a == b;  // see note[1]
};

const arraysEqual = (a, b) => {
  if (a.length !== b.length)
    return false;
  for (let i = 0; i < a.length; i++)
    if (!deepEquals(a[i], b[i]))
      return false;
  return true;
};

const objectsEqual = (a, b) => {
  let aKeys = Object.getOwnPropertyNames(a);
  let bKeys = Object.getOwnPropertyNames(b);
  if (aKeys.length !== bKeys.length)
    return false;
  aKeys.sort();
  bKeys.sort();
  for (let i = 0; i < aKeys.length; i++)
    if (aKeys[i] !== bKeys[i]) // keys must be strings
      return false;
  return deepEquals(aKeys.map(k => a[k]), aKeys.map(k => b[k]));
};

const mapsEqual = (a, b) => {
  if (a.size !== b.size)
    return false;
  let aPairs = Array.from(a);
  let bPairs = Array.from(b);
  aPairs.sort((x, y) => x[0] < y[0]);
  bPairs.sort((x, y) => x[0] < y[0]);
  for (let i = 0; i < a.length; i++)
    if (!deepEquals(aPairs[i][0], bPairs[i][0]) || !deepEquals(aPairs[i][1], bPairs[i][1]))
      return false;
  return true;
};

const typedArraysEqual = (a, b) => {
  a = new Uint8Array(a);
  b = new Uint8Array(b);
  if (a.length !== b.length)
    return false;
  for (let i = 0; i < a.length; i++)
    if (a[i] !== b[i])
      return false;
  return true;
};
