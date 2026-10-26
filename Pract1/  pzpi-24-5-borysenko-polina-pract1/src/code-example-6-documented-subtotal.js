/**
 * Обчислює вартість товарів до застосування знижки.
 *
 * @param {number} unitPrice Ціна однієї одиниці товару.
 * @param {number} quantity Кількість одиниць товару.
 * @returns {number} Загальна вартість.
 */
export function calculateSubtotal(unitPrice, quantity) {
  return unitPrice * quantity;
}
