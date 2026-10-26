import test from "node:test";
import assert from "node:assert/strict";
import { calculateOrderTotal } from "../src/code-example-3-code-after-refactoring.js";

test("обчислює суму без знижки", () => {
  assert.equal(calculateOrderTotal(100, 2), 200);
});

test("обчислює суму зі знижкою", () => {
  assert.equal(calculateOrderTotal(100, 2, 0.1), 180);
});

test("відхиляє від'ємну ціну", () => {
  assert.throws(() => calculateOrderTotal(-1, 2), RangeError);
});

test("відхиляє дробову кількість товару", () => {
  assert.throws(() => calculateOrderTotal(100, 1.5), TypeError);
});
