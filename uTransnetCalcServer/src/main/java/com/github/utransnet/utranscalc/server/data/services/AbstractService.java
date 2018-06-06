package com.github.utransnet.utranscalc.server.data.services;

import org.springframework.data.jpa.repository.JpaRepository;

import javax.transaction.Transactional;
import java.util.List;
import java.util.Optional;

/**
 * Created by Artem on 31.05.2018.
 */
@Transactional
public abstract class AbstractService<T, R extends JpaRepository<T, Long>> {

    protected final R repository;

    public AbstractService(R repository) {
        this.repository = repository;
    }

    public List<T> findAll() {
        return repository.findAll();
    }

    public T saveAndFlush(T entity) {
        return repository.saveAndFlush(entity);
    }

    public T save(T entity) {
        return repository.save(entity);
    }

    public void flush() {
        repository.flush();
    }

    public T getOne(Long aLong) {
        return repository.getOne(aLong);
    }

    public Optional<T> findById(Long aLong) {
        return repository.findById(aLong);
    }

    public long count() {
        return repository.count();
    }

    public void deleteById(Long aLong) {
        repository.deleteById(aLong);
    }

    public void delete(T entity) {
        repository.delete(entity);
    }

    public void deleteAll() {
        repository.deleteAll();
    }
}
