// tariffs.js - Функционал для работы с тарифами
document.addEventListener('DOMContentLoaded', function () {
    // Инициализация элементов
    initTariffFilters();
    initSubscribeButtons();
    initTariffComparison();
    initSearchFunctionality();
});

// Инициализация фильтров тарифов
function initTariffFilters() {
    const filterForm = document.getElementById('tariffFilterForm');
    if (!filterForm) return;

    filterForm.addEventListener('submit', function (e) {
        e.preventDefault();
        applyFilters();
    });

    // Сброс фильтров
    const resetBtn = document.getElementById('resetFilters');
    if (resetBtn) {
        resetBtn.addEventListener('click', function () {
            filterForm.reset();
            applyFilters();
        });
    }
}

// Применение фильтров
async function applyFilters() {
    const minPrice = document.getElementById('minPrice')?.value || 0;
    const maxPrice = document.getElementById('maxPrice')?.value || 5000;
    const minInternet = document.getElementById('minInternet')?.value || 0;
    const minMinutes = document.getElementById('minMinutes')?.value || 0;
    const minSms = document.getElementById('minSms')?.value || 0;

    try {
        // В реальном проекте здесь будет запрос к API
        const response = await fetch(`/api/tariffs/filter?minPrice=${minPrice}&maxPrice=${maxPrice}...`);
        const tariffs = await response.json();

        // Для демонстрации скроем/покажем тарифы на основе фильтров
        const tariffCards = document.querySelectorAll('.tariff-card');

        tariffCards.forEach(card => {
            const price = parseInt(card.dataset.price || 0);
            const internet = parseInt(card.dataset.internet || 0);
            const minutes = parseInt(card.dataset.minutes || 0);
            const sms = parseInt(card.dataset.sms || 0);

            const matchesPrice = price >= minPrice && price <= maxPrice;
            const matchesInternet = internet >= minInternet;
            const matchesMinutes = minutes >= minMinutes;
            const matchesSms = sms >= minSms;

            if (matchesPrice && matchesInternet && matchesMinutes && matchesSms) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });

        showNotification(`Найдено ${document.querySelectorAll('.tariff-card[style="display: block"]').length} тарифов`, 'success');

    } catch (error) {
        console.error('Ошибка фильтрации:', error);
        showNotification('Ошибка при применении фильтров', 'error');
    }
}

// Инициализация кнопок подписки
function initSubscribeButtons() {
    document.querySelectorAll('.subscribe-btn').forEach(button => {
        button.addEventListener('click', async function () {
            const tariffId = this.dataset.tariffId;
            const tariffName = this.dataset.tariffName;

            if (!confirm(`Вы уверены, что хотите подключить тариф "${tariffName}"?`)) {
                return;
            }

            try {
                // Показать индикатор загрузки
                const originalText = this.innerHTML;
                this.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Подключение...';
                this.disabled = true;

                // Отправка запроса на сервер
                const response = await fetch(`/api/tariffs/${tariffId}/subscribe`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    }
                });

                if (response.ok) {
                    showNotification(`Тариф "${tariffName}" успешно подключен!`, 'success');

                    // Обновить UI
                    this.innerHTML = '<i class="bi bi-check-circle"></i> Подключен';
                    this.classList.remove('btn-primary');
                    this.classList.add('btn-success');
                    this.disabled = true;

                    // Обновить баланс в хедере (если есть)
                    updateUserBalance();

                } else {
                    const error = await response.json();
                    throw new Error(error.message || 'Ошибка подключения');
                }

            } catch (error) {
                console.error('Ошибка подключения:', error);
                showNotification(error.message || 'Ошибка при подключении тарифа', 'error');

                // Восстановить кнопку
                this.innerHTML = originalText;
                this.disabled = false;
            }
        });
    });
}

// Инициализация сравнения тарифов
function initTariffComparison() {
    const compareCheckboxes = document.querySelectorAll('.compare-checkbox');
    const compareBtn = document.getElementById('compareTariffsBtn');

    if (!compareBtn) return;

    compareCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            updateCompareButton();
        });
    });

    compareBtn.addEventListener('click', function () {
        const selectedTariffs = Array.from(compareCheckboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value);

        if (selectedTariffs.length < 2) {
            showNotification('Выберите хотя бы 2 тарифа для сравнения', 'warning');
            return;
        }

        if (selectedTariffs.length > 4) {
            showNotification('Можно сравнить не более 4 тарифов', 'warning');
            return;
        }

        // Переход на страницу сравнения
        const queryString = selectedTariffs.map(id => `id=${id}`).join('&');
        window.location.href = `/Tariff/Compare?${queryString}`;
    });
}

function updateCompareButton() {
    const compareBtn = document.getElementById('compareTariffsBtn');
    const selectedCount = document.querySelectorAll('.compare-checkbox:checked').length;

    if (selectedCount >= 2) {
        compareBtn.disabled = false;
        compareBtn.textContent = `Сравнить (${selectedCount})`;
    } else {
        compareBtn.disabled = true;
        compareBtn.textContent = 'Сравнить';
    }
}

// Инициализация поиска
function initSearchFunctionality() {
    const searchInput = document.getElementById('tariffSearch');
    if (!searchInput) return;

    searchInput.addEventListener('input', function () {
        const searchTerm = this.value.toLowerCase().trim();
        const tariffCards = document.querySelectorAll('.tariff-card');

        tariffCards.forEach(card => {
            const title = card.querySelector('.tariff-title')?.textContent.toLowerCase() || '';
            const description = card.querySelector('.tariff-description')?.textContent.toLowerCase() || '';
            const features = card.querySelector('.tariff-features')?.textContent.toLowerCase() || '';

            if (title.includes(searchTerm) || description.includes(searchTerm) || features.includes(searchTerm)) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    });
}

// Обновление баланса пользователя
async function updateUserBalance() {
    try {
        const response = await fetch('/api/user/balance');
        if (response.ok) {
            const data = await response.json();
            const balanceElement = document.querySelector('.user-balance');
            if (balanceElement) {
                balanceElement.textContent = `${data.balance} ₽`;
            }
        }
    } catch (error) {
        console.error('Ошибка обновления баланса:', error);
    }
}

// Всплывающие уведомления
function showNotification(message, type = 'info') {
    // Удаляем старые уведомления
    const oldNotifications = document.querySelectorAll('.custom-notification');
    oldNotifications.forEach(notification => notification.remove());

    // Создаем новое уведомление
    const notification = document.createElement('div');
    notification.className = `custom-notification alert alert-${type} alert-dismissible fade show`;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        animation: slideIn 0.3s ease-out;
    `;

    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    document.body.appendChild(notification);

    // Автоматическое скрытие через 5 секунд
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);

    // Добавляем CSS анимацию
    if (!document.querySelector('#notification-styles')) {
        const style = document.createElement('style');
        style.id = 'notification-styles';
        style.textContent = `
            @keyframes slideIn {
                from { transform: translateX(100%); opacity: 0; }
                to { transform: translateX(0); opacity: 1; }
            }
            @keyframes slideOut {
                from { transform: translateX(0); opacity: 1; }
                to { transform: translateX(100%); opacity: 0; }
            }
        `;
        document.head.appendChild(style);
    }
}

// Расчет стоимости тарифа с учетом услуг
function calculateTotalPrice(tariffId, selectedServices = []) {
    const tariffPrice = parseFloat(document.querySelector(`#tariff_${tariffId} .tariff-price`)?.dataset.price || 0);
    let servicesPrice = 0;

    selectedServices.forEach(serviceId => {
        const servicePrice = parseFloat(document.querySelector(`#service_${serviceId}`)?.dataset.price || 0);
        servicesPrice += servicePrice;
    });

    return tariffPrice + servicesPrice;
}

// Экспорт функций для использования в других скриптах
window.tariffsModule = {
    applyFilters,
    calculateTotalPrice,
    showNotification,
    updateUserBalance
};