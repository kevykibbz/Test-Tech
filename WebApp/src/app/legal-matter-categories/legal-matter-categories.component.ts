import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { LegalMatterCategory } from '../legal-matter-category';
import { LegalMatterCategoryService } from '../legal-matter-category.service';

@Component({
  selector: 'app-legal-matter-categories',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './legal-matter-categories.component.html',
  styleUrls: ['./legal-matter-categories.component.scss']
})
export class LegalMatterCategoriesComponent implements OnInit {
  // State signals
  categories = signal<LegalMatterCategory[]>([]);
  selectedCategory = signal<LegalMatterCategory | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Computed signals
  hasCategories = computed(() => this.categories().length > 0);
  hasSelection = computed(() => !!this.selectedCategory());

  constructor(private categoryService: LegalMatterCategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading.set(true);
    this.error.set(null);
    
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load categories');
        this.loading.set(false);
      }
    });
  }

  onCategorySelect(category: LegalMatterCategory): void {
    this.selectedCategory.set(category);
  }

  onRefresh(): void {
    this.selectedCategory.set(null);
    this.loadCategories();
  }

  trackByCategoryId(index: number, category: LegalMatterCategory): string {
    return category.id;
  }
}
