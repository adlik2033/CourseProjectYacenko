// payments.js - Функционал для работы с платежами
document.addEventListener('DOMContentLoaded', function () {
    // Инициализация компонентов
    initPaymentForm();
    initPaymentHistory();
    initAutoPayments();
    initPaymentCharts();
    initQuickPaymentButtons();
});

// Инициализация формы пополнения баланса
function initPaymentForm() {
    const paymentForm = document.getElementById('paymentForm');
    if (!paymentForm) return;

    const amountInput = document.getElementById('amount');
    const amountDisplay = document.getElementById('amountDisplay');
    const paymentMethodSelect = document.getElementById('paymentMethod');
    const customAmountInput = document.getElementById('customAmount');
    const quickAmountButtons = document.querySelectorAll('.quick-amount');

    // Быстрые суммы
    quickAmountButtons.forEach(button => {
        button.addEventListener('click', function () {
            const amount = this.dataset.amount;
            amountInput.value = amount;
            amountDisplay.textContent = amount;
            customAmountInput.value = '';
            updatePaymentSummary();
        });
    });

    // Кастомная сумма
    customAmountInput.addEventListener('input', function () {
        const value = this.value.trim();
        if (value && !isNaN(value) && parseFloat(value) > 0) {
            amountInput.value = value;
            amountDisplay.textContent = value;
            updatePaymentSummary();
        }
    });

    // Изменение метода оплаты
    paymentMethodSelect.addEventListener('change', function () {
        updatePaymentMethodDetails();
        updatePaymentSummary();
    });

    // Отправка формы
    paymentForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        if (!validatePaymentForm()) {
            return;
        }

        await processPayment();
    });

    // Инициализация начальных значений
    updatePaymentMethodDetails();
    updatePaymentSummary();
}

// Валидация формы оплаты
function validatePaymentForm() {
    const amount = document.getElementById('amount').value;
    const paymentMethod = document.getElementById('paymentMethod').value;

    if (!amount || parseFloat(amount) <= 0) {
        showNotification('Введите корректную сумму', 'error');
        return false;
    }

    if (!paymentMethod) {
        showNotification('Выберите способ оплаты', 'error');
        return false;
    }

    // Дополнительная валидация для кредитной карты
    if (paymentMethod === 'CreditCard') {
        const cardNumber = document.getElementById('cardNumber')?.value;
        const expiryDate = document.getElementById('expiryDate')?.value;
        const cvv = document.getElementById('cvv')?.value;

        if (!cardNumber || cardNumber.replace(/\s/g, '').length !== 16) {
            showNotification('Введите корректный номер карты (16 цифр)', 'error');
            return false;
        }

        if (!expiryDate || !/^\d{2}\/\d{2}$/.test(expiryDate)) {
            showNotification('Введите срок действия в формате ММ/ГГ', 'error');
            return false;
        }

        if (!cvv || cvv.length !== 3) {
            showNotification('Введите CVV код (3 цифры)', 'error');
            return false;
        }
    }

    return true;
}

// Обработка платежа
async function processPayment() {
    const form = document.getElementById('paymentForm');
    const submitBtn = form.querySelector('button[type="submit"]');
    const originalText = submitBtn.innerHTML;

    try {
        // Показать индикатор загрузки
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Обработка...';
        submitBtn.disabled = true;

        // Сбор данных формы
        const formData = new FormData(form);
        const paymentData = Object.fromEntries(formData.entries());

        // Отправка запроса
        const response = await fetch('/api/payment/process', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(paymentData)
        });

        const result = await response.json();

        if (response.ok) {
            showNotification('Платеж успешно проведен!', 'success');

            // Обновление баланса
            updateUserBalance();

            // Очистка формы
            form.reset();
            document.getElementById('amountDisplay').textContent = '0';

            // Обновление истории платежей
            await loadPaymentHistory();

            // Показать квитанцию
            showPaymentReceipt(result.payment);

        } else {
            throw new Error(result.message || 'Ошибка обработки платежа');
        }

    } catch (error) {
        console.error('Ошибка платежа:', error);
        showNotification(error.message || 'Ошибка при проведении платежа', 'error');

    } finally {
        // Восстановить кнопку
        submitBtn.innerHTML = originalText;
        submitBtn.disabled = false;
    }
}

// Обновление деталей метода оплаты
function updatePaymentMethodDetails() {
    const paymentMethod = document.getElementById('paymentMethod').value;
    const detailsContainer = document.getElementById('paymentMethodDetails');

    if (!detailsContainer) return;

    let html = '';

    switch (paymentMethod) {
        case 'CreditCard':
            html = `
                <div class="mb-3">
                    <label for="cardNumber" class="form-label">Номер карты</label>
                    <input type="text" class="form-control" id="cardNumber" 
                           placeholder="1234 5678 9012 3456" maxlength="19"
                           oninput="formatCardNumber(this)">
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <label for="expiryDate" class="form-label">Срок действия</label>
                        <input type="text" class="form-control" id="expiryDate" 
                               placeholder="ММ/ГГ" maxlength="5"
                               oninput="formatExpiryDate(this)">
                    </div>
                    <div class="col-md-6">
                        <label for="cvv" class="form-label">CVV</label>
                        <input type="password" class="form-control" id="cvv" 
                               placeholder="123" maxlength="3">
                    </div>
                </div>
            `;
            break;

        case 'BankTransfer':
            html = `
                <div class="alert alert-info">
                    <h6>Реквизиты для перевода:</h6>
                    <p>Банк: Тинькофф Банк</p>
                    <p>Счет: 40702810123456789012</p>
                    <p>БИК: 044525974</p>
                    <p>Назначение: Пополнение баланса</p>
                </div>
            `;
            break;

        case 'MobilePayment':
            html = `
                <div class="mb-3">
                    <label for="phoneNumber" class="form-label">Номер телефона</label>
                    <input type="tel" class="form-control" id="phoneNumber" 
                           placeholder="+7 (999) 999-99-99">
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" id="savePhone">
                    <label class="form-check-label" for="savePhone">
                        Сохранить номер для будущих платежей
                    </label>
                </div>
            `;
            break;

        default:
            html = '<p>Выберите способ оплаты для продолжения</p>';
    }

    detailsContainer.innerHTML = html;
}

// Обновление сводки платежа
function updatePaymentSummary() {
    const amount = parseFloat(document.getElementById('amount').value) || 0;
    const paymentMethod = document.getElementById('paymentMethod').value;
    const summaryElement = document.getElementById('paymentSummary');

    if (!summaryElement) return;

    let commission = 0;
    let total = amount;

    // Расчет комиссии
    switch (paymentMethod) {
        case 'CreditCard':
            commission = amount * 0.02; // 2%
            break;
        case 'MobilePayment':
            commission = amount * 0.05; // 5%
            break;
        case 'BankTransfer':
            commission = 50; // Фиксированная
            break;
    }

    total = amount + commission;

    summaryElement.innerHTML = `
        <div class="card">
            <div class="card-body">
                <h6>Сводка платежа:</h6>
                <table class="table table-sm">
                    <tr>
                        <td>Сумма пополнения:</td>
                        <td class="text-end">${amount.toFixed(2)} ₽</td>
                    </tr>
                    <tr>
                        <td>Комиссия (${getCommissionPercent(paymentMethod)}):</td>
                        <td class="text-end">${commission.toFixed(2)} ₽</td>
                    </tr>
                    <tr class="table-active fw-bold">
                        <td>Итого к оплате:</td>
                        <td class="text-end">${total.toFixed(2)} ₽</td>
                    </tr>
                </table>
            </div>
        </div>
    `;
}

function getCommissionPercent(method) {
    switch (method) {
        case 'CreditCard': return '2%';
        case 'MobilePayment': return '5%';
        case 'BankTransfer': return '50 ₽';
        default: return '0%';
    }
}

// Инициализация истории платежей
async function initPaymentHistory() {
    const historyTable = document.getElementById('paymentHistoryTable');
    if (!historyTable) return;

    await loadPaymentHistory();

    // Фильтрация истории
    const filterForm = document.getElementById('paymentHistoryFilter');
    if (filterForm) {
        filterForm.addEventListener('submit', async function (e) {
            e.preventDefault();
            await loadPaymentHistory();
        });
    }
}

// Загрузка истории платежей
async function loadPaymentHistory() {
    const historyTableBody = document.getElementById('paymentHistoryTable')?.querySelector('tbody');
    if (!historyTableBody) return;

    try {
        // Показать индикатор загрузки
        historyTableBody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center">
                    <div class="spinner-border spinner-border-sm" role="status">
                        <span class="visually-hidden">Загрузка...</span>
                    </div>
                    Загрузка истории...
                </td>
            </tr>
        `;

        // Получение параметров фильтрации
        const startDate = document.getElementById('startDate')?.value;
        const endDate = document.getElementById('endDate')?.value;
        const statusFilter = document.getElementById('statusFilter')?.value;

        // Формирование URL с параметрами
        let url = '/api/payment/history';
        const params = [];
        if (startDate) params.push(`startDate=${startDate}`);
        if (endDate) params.push(`endDate=${endDate}`);
        if (statusFilter) params.push(`status=${statusFilter}`);
        if (params.length > 0) url += '?' + params.join('&');

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error('Ошибка загрузки истории');
        }

        const payments = await response.json();

        if (payments.length === 0) {
            historyTableBody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center text-muted">
                        <i class="bi bi-receipt fs-1"></i>
                        <p class="mt-2">История платежей пуста</p>
                    </td>
                </tr>
            `;
            return;
        }

        // Отображение платежей
        historyTableBody.innerHTML = payments.map(payment => `
            <tr>
                <td>${formatDateTime(payment.paymentDateTime)}</td>
                <td>${payment.id}</td>
                <td>${payment.amount.toFixed(2)} ₽</td>
                <td>
                    <span class="badge bg-${getStatusBadgeColor(payment.status)}">
                        ${payment.status}
                    </span>
                </td>
                <td>${payment.paymentMethod}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary" 
                            onclick="showPaymentDetails(${payment.id})">
                        <i class="bi bi-receipt"></i> Квитанция
                    </button>
                </td>
            </tr>
        `).join('');

    } catch (error) {
        console.error('Ошибка загрузки истории платежей:', error);
        historyTableBody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center text-danger">
                    <i class="bi bi-exclamation-triangle"></i>
                    <p class="mt-2">Ошибка загрузки истории платежей</p>
                </td>
            </tr>
        `;
    }
}

// Инициализация автоплатежей
function initAutoPayments() {
    const autoPaymentForm = document.getElementById('autoPaymentForm');
    if (!autoPaymentForm) return;

    autoPaymentForm.addEventListener('submit', async function (e) {
        e.preventDefault();
        await setupAutoPayment();
    });

    // Переключение состояния автоплатежа
    document.querySelectorAll('.auto-payment-toggle').forEach(toggle => {
        toggle.addEventListener('change', async function () {
            const autoPaymentId = this.dataset.id;
            const isEnabled = this.checked;

            try {
                const response = await fetch(`/api/payment/auto/${autoPaymentId}/toggle`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({ enabled: isEnabled })
                });

                if (response.ok) {
                    showNotification('Настройки автоплатежа обновлены', 'success');
                } else {
                    throw new Error('Ошибка обновления');
                }

            } catch (error) {
                console.error('Ошибка:', error);
                showNotification('Ошибка при обновлении автоплатежа', 'error');
                this.checked = !isEnabled; // Возвращаем переключатель
            }
        });
    });
}

// Настройка автоплатежа
async function setupAutoPayment() {
    const form = document.getElementById('autoPaymentForm');

    try {
        const formData = new FormData(form);
        const data = Object.fromEntries(formData.entries());

        const response = await fetch('/api/payment/auto/setup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            showNotification('Автоплатеж успешно настроен', 'success');
            form.reset();

            // Обновление списка автоплатежей
            await loadAutoPayments();

        } else {
            const error = await response.json();
            throw new Error(error.message || 'Ошибка настройки');
        }

    } catch (error) {
        console.error('Ошибка настройки автоплатежа:', error);
        showNotification(error.message, 'error');
    }
}

// Инициализация графиков
function initPaymentCharts() {
    const paymentChartCanvas = document.getElementById('paymentChart');
    if (!paymentChartCanvas) return;

    // В реальном проекте здесь будет загрузка данных с сервера
    // и построение графика с помощью Chart.js

    // Пример данных
    const ctx = paymentChartCanvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн'],
            datasets: [{
                label: 'Пополнения баланса',
                data: [500, 1000, 750, 1200, 900, 1500],
                borderColor: 'rgb(75, 192, 192)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                tension: 0.1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Статистика пополнений'
                }
            }
        }
    });
}

// Инициализация быстрых кнопок оплаты
function initQuickPaymentButtons() {
    document.querySelectorAll('.quick-payment-btn').forEach(button => {
        button.addEventListener('click', function () {
            const amount = this.dataset.amount;
            const method = this.dataset.method;

            document.getElementById('amount').value = amount;
            document.getElementById('amountDisplay').textContent = amount;
            document.getElementById('paymentMethod').value = method;

            updatePaymentMethodDetails();
            updatePaymentSummary();

            // Прокрутка к форме
            document.getElementById('paymentForm').scrollIntoView({
                behavior: 'smooth'
            });
        });
    });
}

// Форматирование номера карты
function formatCardNumber(input) {
    let value = input.value.replace(/\D/g, '');
    let formatted = '';

    for (let i = 0; i < value.length && i < 16; i++) {
        if (i > 0 && i % 4 === 0) {
            formatted += ' ';
        }
        formatted += value[i];
    }

    input.value = formatted;
}

// Форматирование даты истечения
function formatExpiryDate(input) {
    let value = input.value.replace(/\D/g, '');

    if (value.length >= 2) {
        value = value.slice(0, 2) + '/' + value.slice(2, 4);
    }

    input.value = value;
}

// Показать квитанцию
function showPaymentReceipt(payment) {
    const receiptModal = new bootstrap.Modal(document.getElementById('receiptModal'));

    document.getElementById('receiptId').textContent = payment.id;
    document.getElementById('receiptDate').textContent = formatDateTime(payment.paymentDateTime);
    document.getElementById('receiptAmount').textContent = `${payment.amount.toFixed(2)} ₽`;
    document.getElementById('receiptMethod').textContent = payment.paymentMethod;
    document.getElementById('receiptStatus').textContent = payment.status;

    receiptModal.show();

    // Кнопка печати
    document.getElementById('printReceipt').onclick = () => {
        window.print();
    };
}

// Показать детали платежа
async function showPaymentDetails(paymentId) {
    try {
        const response = await fetch(`/api/payment/${paymentId}`);
        if (response.ok) {
            const payment = await response.json();
            showPaymentReceipt(payment);
        }
    } catch (error) {
        console.error('Ошибка загрузки деталей:', error);
    }
}

// Загрузка автоплатежей
async function loadAutoPayments() {
    // Реализация загрузки автоплатежей
}

// Вспомогательные функции
function formatDateTime(dateTime) {
    const date = new Date(dateTime);
    return date.toLocaleDateString('ru-RU') + ' ' + date.toLocaleTimeString('ru-RU', {
        hour: '2-digit',
        minute: '2-digit'
    });
}

function getStatusBadgeColor(status) {
    switch (status) {
        case 'Completed': return 'success';
        case 'Pending': return 'warning';
        case 'Failed': return 'danger';
        default: return 'secondary';
    }
}

async function updateUserBalance() {
    try {
        const response = await fetch('/api/user/balance');
        if (response.ok) {
            const data = await response.json();
            const balanceElements = document.querySelectorAll('.user-balance');
            balanceElements.forEach(el => {
                el.textContent = `${data.balance.toFixed(2)} ₽`;
            });
        }
    } catch (error) {
        console.error('Ошибка обновления баланса:', error);
    }
}

function showNotification(message, type = 'info') {
    // Используем ту же функцию, что и в tariffs.js
    if (window.tariffsModule?.showNotification) {
        window.tariffsModule.showNotification(message, type);
    } else {
        alert(message);
    }
}

// Экспорт функций
window.paymentsModule = {
    processPayment,
    loadPaymentHistory,
    showPaymentReceipt,
    updateUserBalance
};