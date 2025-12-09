/**
 * Хук useActiveMenuItem определяет, какой пункт меню считается активным
 * в зависимости от текущего маршрута и параметра scrollTo в URL.
 *
 * Используется для подсветки активного элемента навигации в хедере или сайдбаре.
 * Поддерживает как обычные маршруты (например, '/about'), так и якорные переходы внутри главной страницы.
 *
 * Пример использования:
 * const isActive = useActiveMenuItem();
 * const active = isActive({ href: 'about', scrollTo: true });
 */
import { useLocation } from 'react-router-dom';

export function useActiveMenuItem(scrollParamName = 'scrollTo') {
    const location = useLocation();
    const params = new URLSearchParams(location.search);
    const scrollTo = params.get(scrollParamName);

    return (item: { href: string; scrollTo?: boolean }) => {
        if (item.href === '/') {
            return location.pathname === '/' && !scrollTo;
        }
        if (item.scrollTo) {
            return location.pathname === '/' && scrollTo === item.href;
        }
        return location.pathname === item.href;
    };
}
