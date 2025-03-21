﻿using API_Technical_Test.Datos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using API_Technical_Test.Repositorio.IRepositorio;

namespace API_Technical_Test.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            //Permite hacer consultas
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                //Aplicamos si se recibe como False
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                //Se aplica el filtro a la consulta
                query = query.Where(filtro);
            }
            //Retornar solamente el registro seleccionado
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            //Permite hacer consultas
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                //Se aplica el filtro a la consulta
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }
    }
}
