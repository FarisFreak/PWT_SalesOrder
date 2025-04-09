export const RandomNumber = (minDigits = 2, maxDigits = 5): number => {
  const min = Math.pow(10, minDigits - 1); // contoh: 10 untuk 2 digit
  const max = Math.pow(10, maxDigits) - 1; // contoh: 99999 untuk 5 digit
  return Math.floor(Math.random() * (max - min + 1)) + min;
}
