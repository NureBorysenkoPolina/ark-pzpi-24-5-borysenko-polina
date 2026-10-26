const MAX_DISCOUNT_RATE = 1;

export function calculateOrderTotal(
  unitPrice,
  quantity,
  discountRate = 0
) {
  if (!Number.isFinite(unitPrice)) {
    throw new TypeError("Ціна має бути скінченним числом.");
  }

  if (unitPrice < 0) {
    throw new RangeError("Ціна не може бути від'ємною.");
  }

  if (!Number.isInteger(quantity)) {
    throw new TypeError("Кількість має бути цілим числом.");
  }

  if (quantity < 0) {
    throw new RangeError("Кількість не може бути від'ємною.");
  }

  if (
    !Number.isFinite(discountRate) ||
    discountRate < 0 ||
    discountRate > MAX_DISCOUNT_RATE
  ) {
    throw new RangeError("Знижка має бути в межах від 0 до 1.");
  }

  const subtotal = unitPrice * quantity;
  return subtotal * (1 - discountRate);
}
