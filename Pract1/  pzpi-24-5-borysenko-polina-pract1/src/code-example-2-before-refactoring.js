export function f(p, q, d) {
  let x = p * q;

  if (d) {
    x = x - x * d;
  }

  return x;
}
