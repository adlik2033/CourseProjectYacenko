// Сервис для работы с авторизацией
class AuthService {
    constructor() {
        this.apiBaseUrl = '/api/auth';
        this.token = this.getCookie('access_token');
        this.user = this.getUserFromStorage();
    }

    // Получение токена из cookie
    getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    // Получение пользователя из localStorage
    getUserFromStorage() {
        const userData = localStorage.getItem('user_data');
        return userData ? JSON.parse(userData) : null;
    }

    // Сохранение пользователя
    saveUser(user) {
        localStorage.setItem('user_data', JSON.stringify(user));
        this.user = user;
    }

    // Проверка авторизации
    isAuthenticated() {
        return !!this.token && !!this.user;
    }

    // Проверка роли
    isAdmin() {
        return this.isAuthenticated() && this.user.role === 'Admin';
    }

    // Получение заголовков с авторизацией
    getAuthHeaders() {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (this.token) {
            headers['Authorization'] = `Bearer ${this.token}`;
        }

        return headers;
    }

    // Вход в систему
    async login(phoneNumber, password) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    phoneNumber: phoneNumber,
                    password: password
                })
            });

            if (response.ok) {
                const data = await response.json();
                this.token = data.token;
                this.saveUser(data.user);

                // Сохраняем токен в cookie
                document.cookie = `access_token=${this.token}; path=/; max-age=3600; samesite=strict`;

                return data;
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Ошибка авторизации');
            }
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    }

    // Регистрация
    async register(userData) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(userData)
            });

            if (response.ok) {
                const data = await response.json();
                return data;
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Ошибка регистрации');
            }
        } catch (error) {
            console.error('Registration error:', error);
            throw error;
        }
    }

    // Выход из системы
    async logout() {
        try {
            if (this.isAuthenticated()) {
                await fetch(`${this.apiBaseUrl}/logout`, {
                    method: 'POST',
                    headers: this.getAuthHeaders()
                });
            }
        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            this.clearAuthData();
            window.location.href = '/Account/Login';
        }
    }

    // Получение текущего пользователя
    async getCurrentUser() {
        try {
            const response = await fetch(`${this.apiBaseUrl}/me`, {
                method: 'GET',
                headers: this.getAuthHeaders()
            });

            if (response.ok) {
                const user = await response.json();
                this.saveUser(user);
                return user;
            }
        } catch (error) {
            console.error('Get current user error:', error);
        }

        return null;
    }

    // Очистка данных авторизации
    clearAuthData() {
        this.token = null;
        this.user = null;
        localStorage.removeItem('user_data');
        document.cookie = 'access_token=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }

    // Показ уведомления
    showNotification(message, type = 'info') {
        // Удаляем старые уведомления
        const oldNotifications = document.querySelectorAll('.auth-notification');
        oldNotifications.forEach(el => el.remove());

        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show auth-notification`;
        notification.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            min-width: 300px;
            animation: slideIn 0.3s ease-out;
        `;

        notification.innerHTML = `
            <i class="bi ${type === 'success' ? 'bi-check-circle' : 'bi-exclamation-circle'}"></i>
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        document.body.appendChild(notification);

        // Автоматическое удаление через 5 секунд
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }
}

// Глобальный экземпляр сервиса авторизации
const authService = new AuthService();

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function () {
    // Обработчик кнопки выхода
    const logoutBtn = document.getElementById('logoutBtn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', async (e) => {
            e.preventDefault();
            await authService.logout();
        });
    }

    // Обработчик формы входа
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const phoneNumber = document.getElementById('PhoneNumber')?.value;
            const password = document.getElementById('Password')?.value;

            if (!phoneNumber || !password) {
                authService.showNotification('Заполните все поля', 'error');
                return;
            }

            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;

            try {
                submitBtn.disabled = true;
                submitBtn.textContent = 'Вход...';

                await authService.login(phoneNumber, password);

                // Перенаправляем на главную страницу
                window.location.href = '/Home/Dashboard';

            } catch (error) {
                authService.showNotification(error.message, 'error');
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;
            }
        });
    }

    // Обработчик формы регистрации
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = {
                fullName: document.getElementById('FullName')?.value,
                phoneNumber: document.getElementById('PhoneNumber')?.value,
                email: document.getElementById('Email')?.value,
                address: document.getElementById('Address')?.value,
                passportData: document.getElementById('PassportData')?.value,
                password: document.getElementById('Password')?.value,
                confirmPassword: document.getElementById('ConfirmPassword')?.value
            };

            // Проверка паролей
            if (formData.password !== formData.confirmPassword) {
                authService.showNotification('Пароли не совпадают', 'error');
                return;
            }

            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;

            try {
                submitBtn.disabled = true;
                submitBtn.textContent = 'Регистрация...';

                const result = await authService.register(formData);

                authService.showNotification('Регистрация успешна! Теперь вы можете войти.', 'success');

                // Перенаправляем на страницу входа через 2 секунды
                setTimeout(() => {
                    window.location.href = '/Account/Login';
                }, 2000);

            } catch (error) {
                authService.showNotification(error.message, 'error');
                submitBtn.disabled = false;
                submitBtn.textContent = originalText;
            }
        });
    }

    // Автоформат номера телефона
    const phoneInputs = document.querySelectorAll('input[type="tel"], input[name*="phone"]');
    phoneInputs.forEach(input => {
        input.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');

            if (value.startsWith('7')) {
                value = '+7' + value.substring(1);
            } else if (value.startsWith('8')) {
                value = '+7' + value.substring(1);
            } else if (value.length > 0 && !value.startsWith('+')) {
                value = '+7' + value;
            }

            // Ограничение длины
            if (value.length > 12) {
                value = value.substring(0, 12);
            }

            e.target.value = value;
        });
    });

    // Валидация пароля
    const passwordInputs = document.querySelectorAll('input[type="password"]');
    passwordInputs.forEach(input => {
        input.addEventListener('blur', function () {
            const value = this.value;
            if (value && value.length < 6) {
                this.classList.add('is-invalid');
                const feedback = this.nextElementSibling;
                if (feedback && feedback.classList.contains('invalid-feedback')) {
                    feedback.textContent = 'Пароль должен содержать не менее 6 символов';
                }
            } else {
                this.classList.remove('is-invalid');
            }
        });
    });
});

// Добавляем CSS анимацию для уведомлений
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(100%);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    .auth-notification {
        animation: slideIn 0.3s ease-out;
    }
`;
document.head.appendChild(style);