import { Component, OnInit } from '@angular/core';
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
  categories$!: Observable<LegalMatterCategory[]>;
  selectedCategory: LegalMatterCategory | null = null;
  loading = false;
  error: string | null = null;

  constructor(private categoryService: LegalMatterCategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading = true;
    this.error = null;
    this.categories$ = this.categoryService.getCategories();
  }

  onCategorySelect(category: LegalMatterCategory): void {
    this.selectedCategory = category;
  }

  onRefresh(): void {
    this.selectedCategory = null;
    this.loadCategories();
  }

  trackByCategoryId(index: number, category: LegalMatterCategory): string {
    return category.id;
  }
}
