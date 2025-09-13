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
