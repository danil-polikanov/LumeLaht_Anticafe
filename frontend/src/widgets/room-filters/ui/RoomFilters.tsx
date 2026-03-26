import React, { useState } from 'react';
import * as Popover from '@radix-ui/react-popover';
import * as Slider from '@radix-ui/react-slider';
import { AnimatePresence, motion } from 'framer-motion';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
import { resetFilters, setFilters } from '@/entities/room/model';
import { selectFilters } from '@/entities/room/';
import { useGetActivitiesQuery } from '@/entities/room/';
import { useDebounce } from '@/shared/lib/hooks/useDebounce';
import styles from './RoomFilters.module.css';

const ACTIVITY_ICONS: Record<string, { icon: string; bg: string; text: string }> = {
  default: { icon: 'fas fa-star', bg: 'bg-gray-100', text: 'text-gray-600' },
};

const popoverAnimation = {
  initial: { opacity: 0, scale: 0.96, y: -4 },
  animate: { opacity: 1, scale: 1, y: 0 },
  exit: { opacity: 0, scale: 0.96, y: -4 },
  transition: { duration: 0.15 },
};

export const RoomFilters: React.FC = () => {
  const dispatch = useAppDispatch();
  const filters = useAppSelector(selectFilters);
  const { data: activities = [] } = useGetActivitiesQuery();

  // Local state for debounced search
  const [localSearch, setLocalSearch] = useState(filters.search);
  const debouncedSetSearch = useDebounce((value: string) => {
    dispatch(setFilters({ search: value }));
  }, 500);

  // Local state for city/region search inside popovers
  const [cityInput, setCityInput] = useState('');
  const [regionInput, setRegionInput] = useState('');

  // Price slider local state
  const [priceRange, setPriceRange] = useState<[number, number]>([
    filters.minPrice || 0,
    filters.maxPrice === 10000 ? 50 : filters.maxPrice,
  ]);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setLocalSearch(value);
    debouncedSetSearch(value);
  };

  const handleCitySelect = (city: string) => {
    dispatch(setFilters({ city }));
  };

  const handleRegionSelect = (region: string) => {
    dispatch(setFilters({ region }));
  };

  const handlePriceCommit = (values: number[]) => {
    dispatch(setFilters({ minPrice: values[0], maxPrice: values[1] }));
  };

  const handleActivityToggle = (activityId: string) => {
    const newActivities = filters.activitiesIds.includes(activityId)
      ? filters.activitiesIds.filter((id) => id !== activityId)
      : [...filters.activitiesIds, activityId];
    dispatch(setFilters({ activitiesIds: newActivities }));
  };

  const clearFilters = () => {
    dispatch(resetFilters());
    setLocalSearch('');
    setPriceRange([0, 50]);
  };

  const removeFilter = (key: string, value?: string) => {
    if (key === 'city') dispatch(setFilters({ city: '' }));
    if (key === 'region') dispatch(setFilters({ region: '' }));
    if (key === 'price') {
      dispatch(setFilters({ minPrice: 0, maxPrice: 10000 }));
      setPriceRange([0, 50]);
    }
    if (key === 'activity' && value) {
      dispatch(
        setFilters({
          activitiesIds: filters.activitiesIds.filter((id) => id !== value),
        }),
      );
    }
  };

  const activeActivityNames = activities.filter((a) =>
    filters.activitiesIds.includes(a.activityId ?? ''),
  );
  const hasActiveFilters =
    filters.search || filters.city || filters.region ||
    filters.minPrice > 0 || (filters.maxPrice < 10000 && filters.maxPrice > 0) ||
    filters.activitiesIds.length > 0;

  // Cities/Regions — collected from known values
  const knownCities = ['Tallinn', 'Tartu', 'Pärnu', 'Narva', 'Viljandi'];
  const knownRegions = ['Harju', 'Tartu', 'Pärnu', 'Ida-Viru'];
  const filteredCities = knownCities.filter((c) =>
    c.toLowerCase().includes(cityInput.toLowerCase()),
  );
  const filteredRegions = knownRegions.filter((r) =>
    r.toLowerCase().includes(regionInput.toLowerCase()),
  );

  return (
    <div>
      {/* Search bar */}
      <div className={styles.searchBar}>
        <i className={`fas fa-search ${styles.searchIcon}`}></i>
        <input
          type="text"
          placeholder="Search rooms by name or description..."
          className={styles.searchInput}
          value={localSearch}
          onChange={handleSearchChange}
        />
        {localSearch !== filters.search && (
          <div className={styles.liveBadge}>
            <div className={styles.liveDot}></div>
            <span className={styles.liveText}>Applying...</span>
          </div>
        )}
      </div>

      {/* Filter pills row */}
      <div className={styles.pillRow}>
        {/* City */}
        <FilterPill
          icon="fas fa-map-marker-alt"
          label="City"
          value={filters.city}
          popoverWidth={220}
        >
          <div className={styles.selectSearch}>
            <div className={styles.selectSearchInner}>
              <i className={`fas fa-search ${styles.selectSearchIcon}`}></i>
              <input
                type="text"
                placeholder="Search city..."
                className={styles.selectSearchInput}
                value={cityInput}
                onChange={(e) => setCityInput(e.target.value)}
              />
            </div>
          </div>
          <div className={styles.selectList}>
            {filteredCities.map((city) => (
              <div
                key={city}
                className={`${styles.selectItem} ${filters.city === city ? styles.selectItemSelected : ''}`}
                onClick={() => handleCitySelect(city)}
              >
                <i className={`fas fa-map-pin ${styles.selectItemIcon} ${filters.city === city ? 'text-accent' : 'text-gray-300'}`}></i>
                <span>{city}</span>
                {filters.city === city && <i className={`fas fa-check ${styles.selectItemCheck}`}></i>}
              </div>
            ))}
          </div>
        </FilterPill>

        {/* Region */}
        <FilterPill
          icon="fas fa-globe"
          label="Region"
          value={filters.region}
          popoverWidth={200}
        >
          <div className={styles.selectSearch}>
            <div className={styles.selectSearchInner}>
              <i className={`fas fa-search ${styles.selectSearchIcon}`}></i>
              <input
                type="text"
                placeholder="Search region..."
                className={styles.selectSearchInput}
                value={regionInput}
                onChange={(e) => setRegionInput(e.target.value)}
              />
            </div>
          </div>
          <div className={styles.selectList}>
            {filteredRegions.map((region) => (
              <div
                key={region}
                className={`${styles.selectItem} ${filters.region === region ? styles.selectItemSelected : ''}`}
                onClick={() => handleRegionSelect(region)}
              >
                <span>{region}</span>
                {filters.region === region && <i className={`fas fa-check ${styles.selectItemCheck}`}></i>}
              </div>
            ))}
          </div>
        </FilterPill>

        {/* Price */}
        <FilterPill
          icon="fas fa-euro-sign"
          label="Price"
          value={
            filters.minPrice > 0 || (filters.maxPrice < 10000 && filters.maxPrice > 0)
              ? `${filters.minPrice} — ${filters.maxPrice === 10000 ? '50+' : filters.maxPrice} €/h`
              : ''
          }
          popoverWidth={300}
        >
          <div className={styles.pricePopover}>
            <div className={styles.priceHeader}>
              <span className={styles.priceLabel}>Price per hour</span>
              <span className={styles.priceValue}>{priceRange[0]}€ — {priceRange[1]}€</span>
            </div>
            <Slider.Root
              className={styles.sliderRoot}
              min={0}
              max={50}
              step={1}
              value={priceRange}
              onValueChange={(v) => setPriceRange(v as [number, number])}
              onValueCommit={handlePriceCommit}
              minStepsBetweenThumbs={1}
            >
              <Slider.Track className={styles.sliderTrack}>
                <Slider.Range className={styles.sliderRange} />
              </Slider.Track>
              <Slider.Thumb className={styles.sliderThumb} />
              <Slider.Thumb className={styles.sliderThumb} />
            </Slider.Root>
            <div className={styles.sliderBounds}>
              <span>0€</span>
              <span>50€</span>
            </div>
            <div className={styles.priceInputs}>
              <div className={styles.priceInputWrap}>
                <label className={styles.priceInputLabel}>Min</label>
                <div className={styles.priceInputField}>
                  <span className={styles.priceInputCurrency}>€</span>
                  <input
                    type="number"
                    className={styles.priceInputEl}
                    value={priceRange[0]}
                    onChange={(e) => {
                      const v = Number(e.target.value);
                      const newRange: [number, number] = [v, priceRange[1]];
                      setPriceRange(newRange);
                      handlePriceCommit(newRange);
                    }}
                  />
                </div>
              </div>
              <div className={styles.priceInputDash}>—</div>
              <div className={styles.priceInputWrap}>
                <label className={styles.priceInputLabel}>Max</label>
                <div className={styles.priceInputField}>
                  <span className={styles.priceInputCurrency}>€</span>
                  <input
                    type="number"
                    className={styles.priceInputEl}
                    value={priceRange[1]}
                    onChange={(e) => {
                      const v = Number(e.target.value);
                      const newRange: [number, number] = [priceRange[0], v];
                      setPriceRange(newRange);
                      handlePriceCommit(newRange);
                    }}
                  />
                </div>
              </div>
            </div>
          </div>
        </FilterPill>

        {/* Activities */}
        <FilterPill
          icon="fas fa-gamepad"
          label="Activities"
          badgeCount={filters.activitiesIds.length}
          popoverWidth={380}
        >
          <div className={styles.activitiesPopover}>
            <div className={styles.activitiesHeader}>
              <span className={styles.activitiesTitle}>Select activities</span>
              {filters.activitiesIds.length > 0 && (
                <button
                  className={styles.activitiesClear}
                  onClick={() => dispatch(setFilters({ activitiesIds: [] }))}
                >
                  Clear
                </button>
              )}
            </div>
            <div className={styles.activitiesGrid}>
              {activities.map((activity) => {
                const isOn = filters.activitiesIds.includes(activity.activityId ?? '');
                const meta = ACTIVITY_ICONS[activity.name ?? ''] ?? ACTIVITY_ICONS.default;
                return (
                  <div
                    key={activity.activityId}
                    className={`${styles.actToggle} ${isOn ? styles.actToggleOn : ''}`}
                    onClick={() => handleActivityToggle(activity.activityId ?? '')}
                  >
                    <div
                      className={`${styles.actIcon} ${isOn ? styles.actIconOn : meta.bg + ' ' + meta.text}`}
                    >
                      <i className={meta.icon}></i>
                    </div>
                    <span className={styles.actName}>{activity.name}</span>
                    <div className={`${styles.actCheck} ${isOn ? styles.actCheckOn : ''}`}>
                      {isOn && <i className="fas fa-check"></i>}
                    </div>
                  </div>
                );
              })}
            </div>
          </div>
        </FilterPill>

        {/* Clear all */}
        {hasActiveFilters && (
          <button className={styles.clearBtn} onClick={clearFilters}>
            <i className="fas fa-times text-xs"></i>
            <span>Clear all</span>
          </button>
        )}
      </div>

      {/* Active filters strip */}
      {hasActiveFilters && (
        <div className={styles.activeStrip}>
          <span className={styles.activeLabel}>
            <i className="fas fa-filter mr-1"></i>Active:
          </span>
          {filters.city && (
            <span className={styles.activeBadge}>
              <i className={`fas fa-map-marker-alt ${styles.activeBadgeIcon}`}></i>
              {filters.city}
              <button className={styles.activeBadgeRemove} onClick={() => removeFilter('city')}>
                &times;
              </button>
            </span>
          )}
          {filters.region && (
            <span className={styles.activeBadge}>
              <i className={`fas fa-globe ${styles.activeBadgeIcon}`}></i>
              {filters.region}
              <button className={styles.activeBadgeRemove} onClick={() => removeFilter('region')}>
                &times;
              </button>
            </span>
          )}
          {(filters.minPrice > 0 || (filters.maxPrice < 10000 && filters.maxPrice > 0)) && (
            <span className={styles.activeBadge}>
              <i className={`fas fa-euro-sign ${styles.activeBadgeIcon}`}></i>
              {filters.minPrice}—{filters.maxPrice === 10000 ? '50+' : filters.maxPrice}€
              <button className={styles.activeBadgeRemove} onClick={() => removeFilter('price')}>
                &times;
              </button>
            </span>
          )}
          {activeActivityNames.map((a) => (
            <span key={a.activityId} className={styles.activeBadge}>
              <i className={`fas fa-gamepad ${styles.activeBadgeIcon}`}></i>
              {a.name}
              <button
                className={styles.activeBadgeRemove}
                onClick={() => removeFilter('activity', a.activityId ?? '')}
              >
                &times;
              </button>
            </span>
          ))}
        </div>
      )}
    </div>
  );
};

/* ===== Reusable Filter Pill with Radix Popover ===== */
interface FilterPillProps {
  icon: string;
  label: string;
  value?: string;
  badgeCount?: number;
  popoverWidth?: number;
  children: React.ReactNode;
}

const FilterPill: React.FC<FilterPillProps> = ({
  icon,
  label,
  value,
  badgeCount,
  popoverWidth,
  children,
}) => {
  const [open, setOpen] = useState(false);
  const hasValue = !!value || (badgeCount && badgeCount > 0);

  return (
    <Popover.Root open={open} onOpenChange={setOpen}>
      <Popover.Trigger asChild>
        <button className={`${styles.pill} ${hasValue ? styles.pillHasValue : ''}`}>
          <i className={`${icon} ${styles.pillIcon}`}></i>
          {value ? (
            <span className={styles.pillValue}>{value}</span>
          ) : (
            <span className={styles.pillLabel}>{label}</span>
          )}
          {badgeCount && badgeCount > 0 ? (
            <span className={styles.pillBadge}>{badgeCount}</span>
          ) : null}
          <i className={`fas fa-chevron-down ${styles.pillChevron}`}></i>
        </button>
      </Popover.Trigger>
      <AnimatePresence>
        {open && (
          <Popover.Portal forceMount>
            <Popover.Content
              asChild
              sideOffset={8}
              align="center"
              onOpenAutoFocus={(e) => e.preventDefault()}
            >
              <motion.div
                className={styles.popoverContent}
                style={{ width: popoverWidth }}
                {...popoverAnimation}
              >
                <Popover.Arrow className={styles.popoverArrow} />
                {children}
              </motion.div>
            </Popover.Content>
          </Popover.Portal>
        )}
      </AnimatePresence>
    </Popover.Root>
  );
};
