// Сервис для работы с профилем
class ProfileService {
    constructor() {
        this.apiBaseUrl = '/api/profile';
        this.token = this.getCookie('access_token');
    }

    getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    getAuthHeaders() {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (this.token) {
            headers['Authorization'] = `Bearer ${this.token}`;
        }

        return headers;
    }

    // Получение профиля пользователя
    async getProfile() {
        try {
            const response = await fetch(`${this.apiBaseUrl}`, {
                method: 'GET',
                headers: this.getAuthHeaders()
            });

            if (response.ok) {
                return await response.json();
            }
            return null;
        } catch (error) {
            console.error('Get profile error:', error);
            return null;
        }
    }

    // Обновление профиля
    async updateProfile(profileData) {
        try {
            const response = await fetch(`${this.apiBaseUrl}`, {
                method: 'PUT',
                headers: this.getAuthHeaders(),
                body: JSON.stringify(profileData)
            });

            return response.ok;
        } catch (error) {
            console.error('Update profile error:', error);
            return false;
        }
    }

    // Получение тарифов пользователя
    async getUserTariffs() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/tariffs`, {
                method: 'GET',
                headers: this.getAuthHeaders()
            });

            if (response.ok) {
                return await response.json();
            }
            return [];
        } catch (error) {
            console.error('Get tariffs error:', error);
            return [];
        }
    }

    // Подключение тарифа
    async subscribeTariff(tariffId) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/tariffs/subscribe`, {
                method: 'POST',
                headers: this.getAuthHeaders(),
                body: JSON.stringify({ tariffId: tariffId })
            });

            return response.ok;
        } catch (error) {
            console.error('Subscribe tariff error:', error);
            return false;
        }
    }

    // Отключение тарифа
    async unsubscribeTariff(tariffId) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/tariffs/unsubscribe`, {
                method: 'POST',
                headers: this.getAuthHeaders(),
                body: JSON.stringify({ tariffId: tariffId })
            });

            return response.ok;
        } catch (error) {
            console.error('Unsubscribe tariff error:', error);
            return false;
        }
    }

    // Показ уведомления
    showNotification(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show`;
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            min-width: 300px;
        `;

        const icon = type === 'success' ? 'bi-check-circle' : 'bi-exclamation-circle';
        notification.innerHTML = `
            <i class="bi ${icon}"></i> ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        document.body.appendChild(notification);

        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }

    // Форматирование даты
    formatDate(dateString) {
        const date = new Date(dateString);
        return date.toLocaleDateString('ru-RU', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    }

    // Форматирование суммы
    formatAmount(amount) {
        return new Intl.NumberFormat('ru-RU', {
            style: 'currency',
            currency: 'RUB',
            minimumFractionDigits: 0
        }).format(amount);
    }
}

// Глобальный экземпляр сервиса профиля
const profileService = new ProfileService();

// Инициализация страницы профиля
document.addEventListener('DOMContentLoaded', function () {
    // Загрузка данных профиля
    loadProfileData();

    // Загрузка тарифов пользователя
    loadUserTariffs();

    // Обработчики форм
    initFormHandlers();

    // Обработчики кнопок тарифов
    initTariffButtons();
});

// Загрузка данных профиля
async function loadProfileData() {
    const profileElement = document.getElementById('profile-data');
    if (!profileElement) return;

    try {
        const profile = await profileService.getProfile();
        if (profile) {
            updateProfileUI(profile);
        }
    } catch (error) {
        console.error('Error loading profile:', error);
    }
}

// Обновление UI профиля
function updateProfileUI(profile) {
    // Обновляем основные поля
    const fields = {
        'user-fullname': profile.fullName,
        'user-phone': profile.phoneNumber,
        'user-email': profile.email,
        'user-address': profile.address,
        'user-passport': profile.passportData,
        'user-regdate': profileService.formatDate(profile.registrationDate),
        'user-balance': profileService.formatAmount(profile.balance),
        'user-status': profile.isActive ? 'Активен' : 'Заблокирован'
    };

    Object.entries(fields).forEach(([id, value]) => {
        const element = document.getElementById(id);
        if (element) {
            element.textContent = value;
        }
    });

    // Обновляем статус
    const statusElement = document.getElementById('user-status-badge');
    if (statusElement) {
        statusElement.className = `badge ${profile.isActive ? 'bg-success' : 'bg-danger'}`;
        statusElement.textContent = profile.isActive ? 'Активен' : 'Заблокирован';
    }
}

// Загрузка тарифов пользователя
async function loadUserTariffs() {
    const tariffsContainer = document.getElementById('user-tariffs');
    if (!tariffsContainer) return;

    try {
        const tariffs = await profileService.getUserTariffs();
        if (tariffs.length > 0) {
            renderTariffs(tariffs);
        } else {
            tariffsContainer.innerHTML = `
                <div class="col-12">
                    <div class="alert alert-info">
                        <h5>У вас пока нет подключенных тарифов</h5>
                        <p>Перейдите в каталог тарифов, чтобы выбрать подходящий вариант.</p>
                        <a href="/Tariff" class="btn btn-primary">Выбрать тариф</a>
                    </div>
                </div>
            `;
        }
    } catch (error) {
        console.error('Error loading tariffs:', error);
    }
}

// Отрисовка тарифов
function renderTariffs(tariffs) {
    const container = document.getElementById('user-tariffs');
    if (!container) return;

    const html = tariffs.map(tariff => `
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow">
                <div class="card-header bg-primary text-white">
                    <h5 class="card-title mb-0">${tariff.name}</h5>
                </div>
                <div class="card-body">
                    <p class="card-text">${tariff.description || ''}</p>
                    <div class="mb-3">
                        <span class="h4">${tariff.monthlyFee.toFixed(0)} руб./мес</span>
                    </div>
                    <ul class="list-group list-group-flush mb-3">
                        <li class="list-group-item d-flex justify-content-between">
                            <span>Интернет:</span>
                            <strong>${tariff.internetTrafficGB} ГБ</strong>
                        </li>
                        <li class="list-group-item d-flex justify-content-between">
                            <span>Минуты:</span>
                            <strong>${tariff.minutesCount} мин</strong>
                        </li>
                        <li class="list-group-item d-flex justify-content-between">
                            <span>SMS:</span>
                            <strong>${tariff.smsCount} шт</strong>
                        </li>
                    </ul>
                </div>
                <div class="card-footer">
                    <button class="btn btn-danger w-100 unsubscribe-btn" 
                            data-tariff-id="${tariff.id}">
                        Отключить тариф
                    </button>
                </div>
            </div>
        </div>
    `).join('');

    container.innerHTML = html;

    // Добавляем обработчики для кнопок отключения
    document.querySelectorAll('.unsubscribe-btn').forEach(button => {
        button.addEventListener('click', async function () {
            const tariffId = this.getAttribute('data-tariff-id');
            if (confirm('Вы уверены, что хотите отключить этот тариф?')) {
                const success = await profileService.unsubscribeTariff(tariffId);
                if (success) {
                    profileService.showNotification('Тариф успешно отключен', 'success');
                    setTimeout(() => location.reload(), 1500);
                } else {
                    profileService.showNotification('Ошибка при отключении тарифа', 'error');
                }
            }
        });
    });
}

// Инициализация обработчиков форм
function initFormHandlers() {
    // Форма редактирования профиля
    const editForm = document.getElementById('edit-profile-form');
    if (editForm) {
        editForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = {
                fullName: document.getElementById('edit-fullname')?.value,
                email: document.getElementById('edit-email')?.value,
                address: document.getElementById('edit-address')?.value,
                passportData: document.getElementById('edit-passport')?.value
            };

            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;

            try {
                submitBtn.disabled = true;
                submitBtn.textContent = 'Сохранение...';

                const success = await profileService.updateProfile(formData);

                if (success) {
                    profileService.showNotification('Профиль успешно обновлен', 'success');
                    setTimeout(() => location.reload(), 1500);
                } else {
                    profileService.showNotification('Ошибка при обновлении профиля', 'error');
                    submitBtn.disabled = false;
                    submitBtn.textContent = originalText;
                }
            } catch (error) {
                profileService.showNotification('Ошибка при обновлении профиля', 'error');
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;
            }
        });
    }

    // Форма смены пароля
    const changePasswordForm = document.getElementById('change-password-form');
    if (changePasswordForm) {
        changePasswordForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const currentPassword = document.getElementById('current-password')?.value;
            const newPassword = document.getElementById('new-password')?.value;
            const confirmPassword = document.getElementById('confirm-password')?.value;

            if (newPassword !== confirmPassword) {
                profileService.showNotification('Новые пароли не совпадают', 'error');
                return;
            }

            if (newPassword.length < 6) {
                profileService.showNotification('Пароль должен содержать не менее 6 символов', 'error');
                return;
            }

            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;

            try {
                submitBtn.disabled = true;
                submitBtn.textContent = 'Смена пароля...';

                // Здесь будет вызов API для смены пароля
                profileService.showNotification('Функция смены пароля временно недоступна', 'warning');
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;

            } catch (error) {
                profileService.showNotification('Ошибка при смене пароля', 'error');
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;
            }
        });
    }
}

// Инициализация кнопок тарифов
function initTariffButtons() {
    // Кнопки подписки на тарифы (на странице тарифов)
    document.querySelectorAll('.subscribe-btn').forEach(button => {
        button.addEventListener('click', async function () {
            const tariffId = this.getAttribute('data-tariff-id');
            const tariffName = this.getAttribute('data-tariff-name');

            if (confirm(`Подключить тариф "${tariffName}"?`)) {
                const success = await profileService.subscribeTariff(tariffId);
                if (success) {
                    profileService.showNotification(`Тариф "${tariffName}" успешно подключен`, 'success');
                    setTimeout(() => {
                        window.location.href = '/Profile/MyTariffs';
                    }, 1500);
                } else {
                    profileService.showNotification('Ошибка при подключении тарифа', 'error');
                }
            }
        });
    });
}

// Управление подписками
function manageSubscriptions() {
    const manageButtons = document.querySelectorAll('.manage-subscription');
    manageButtons.forEach(button => {
        button.addEventListener('click', function () {
            const serviceId = this.getAttribute('data-service-id');
            const action = this.getAttribute('data-action');

            if (action === 'activate') {
                activateService(serviceId);
            } else if (action === 'deactivate') {
                deactivateService(serviceId);
            }
        });
    });
}

// Активация услуги
async function activateService(serviceId) {
    try {
        profileService.showNotification('Услуга активирована', 'success');
    } catch (error) {
        profileService.showNotification('Ошибка активации услуги', 'error');
    }
}

// Деактивация услуги
async function deactivateService(serviceId) {
    try {
        profileService.showNotification('Услуга деактивирована', 'success');
    } catch (error) {
        profileService.showNotification('Ошибка деактивации услуги', 'error');
    }
}